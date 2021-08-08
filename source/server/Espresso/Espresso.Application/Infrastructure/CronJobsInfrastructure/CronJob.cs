﻿// CronJob.cs
//
// © 2021 Espresso News. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Cronos;
using Espresso.Application.Services.Contracts;
using Espresso.Domain.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace Espresso.Application.Infrastructure.CronJobsInfrastructure
{
    /// <summary>
    /// Represents scheduled background job.
    /// </summary>
    /// <typeparam name="T">Cron job.</typeparam>
    public abstract class CronJob<T> : IHostedService, IDisposable
        where T : CronJob<T>
    {
        private readonly CronExpression _expression;
        private readonly ICronJobConfiguration<T> _cronJobConfiguration;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer? _timer;
        private bool _disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CronJob{T}"/> class.
        /// </summary>
        /// <param name="cronJobConfiguration">Cron job configuration.</param>
        /// <param name="serviceScopeFactory">Service scope factory.</param>
        protected CronJob(
            ICronJobConfiguration<T> cronJobConfiguration,
            IServiceScopeFactory serviceScopeFactory
        )
        {
            _expression = CronExpression.Parse(cronJobConfiguration.CronExpression);
            _cronJobConfiguration = cronJobConfiguration;
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <inheritdoc />
        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            return ScheduleJob(cancellationToken);
        }

        /// <summary>
        /// Schedules cron job.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var occurrence = _expression.GetNextOccurrence(
                from: DateTimeOffset.Now,
                zone: _cronJobConfiguration.TimeZoneInfo
            );

            if (!occurrence.HasValue)
            {
                return;
            }

            var delay = occurrence.Value - DateTimeOffset.Now;

            if (delay.TotalMilliseconds <= 0)
            {
                await ScheduleJob(cancellationToken);
            }

            _timer = new Timer(delay.TotalMilliseconds);
#pragma warning disable AsyncFixer03, VSTHRD101 // Avoid unsupported fire-and-forget async-void methods or delegates. Unhandled exceptions will crash the process
            _timer.Elapsed += async (sender, args) =>
            {
                _timer?.Dispose();
                _timer = null;

                await ExecuteWork(
                    occurrence: occurrence.Value,
                    cancellationToken: cancellationToken
                );
            };
#pragma warning restore AsyncFixer03, VSTHRD101
            _timer.Start();

            var eventName = $"{typeof(T).Name} scheduled";
            var arguments = new List<(string argumentName, object argumentValue)>
            {
                (nameof(occurrence), occurrence),
            };

            using var scope = _serviceScopeFactory.CreateScope();
            var loggerService = scope.ServiceProvider.GetRequiredService<ILoggerService<CronJob<T>>>();

            loggerService.Log(eventName, LogLevel.Information, arguments);
        }

        /// <summary>
        /// Cron job work.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public abstract Task DoWork(CancellationToken cancellationToken);

        /// <inheritdoc />
#pragma warning disable RCS1229 // Use async/await when necessary
        public virtual Task StopAsync(CancellationToken cancellationToken)
#pragma warning restore RCS1229
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var loggerService = scope.ServiceProvider.GetRequiredService<ILoggerService<CronJob<T>>>();

            var eventName = $"{typeof(T).Name} stopped";
            loggerService.Log(eventName, LogLevel.Information);

            _timer?.Stop();

            return Task.CompletedTask;
        }

        private async Task ExecuteWork(
            DateTimeOffset occurrence,
            CancellationToken cancellationToken
        )
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            using var scope = _serviceScopeFactory.CreateScope();
            var loggerService = scope.ServiceProvider.GetRequiredService<ILoggerService<CronJob<T>>>();

            try
            {
                var stopwatch = Stopwatch.StartNew();
                await DoWork(cancellationToken);
                var elapsed = stopwatch.Elapsed;

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

                var eventName = $"{typeof(T).Name} work ended";
                var nextOccurrence = _expression.GetNextOccurrence(DateTimeOffset.Now, _cronJobConfiguration.TimeZoneInfo) ?? occurrence;

                var arguments = new (string argumentName, object argumentValue)[]
                {
                    (nameof(occurrence), occurrence),
                    (nameof(nextOccurrence), nextOccurrence),
                    (nameof(elapsed), elapsed),
                };
                loggerService.Log(eventName, LogLevel.Information, arguments);
            }
            catch (Exception exception)
            {
                var eventName = $"{typeof(T).Name} error";

                var arguments = new (string argumentName, object argumentValue)[]
                {
                    (nameof(occurrence), occurrence),
                };

                loggerService.Log(eventName, exception, LogLevel.Error, arguments);

                var slackService = scope.ServiceProvider.GetRequiredService<ISlackService>();
                await slackService.LogError(
                    eventName: eventName,
                    message: exception.Message,
                    exception: exception,
                    cancellationToken: default
                );
            }

            if (!cancellationToken.IsCancellationRequested)
            {
                await ScheduleJob(cancellationToken);
            }
        }

        /// <summary>
        /// Performs application defined tasks associated with freeing,
        /// releasing, or reseting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _timer?.Dispose();
                }

                _disposedValue = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
