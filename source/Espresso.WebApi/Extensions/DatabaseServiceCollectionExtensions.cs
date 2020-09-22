﻿using Espresso.Common.Enums;

using Espresso.Persistence.Database;
using Espresso.Persistence.IRepositories;
using Espresso.Persistence.Repositories;
using Espresso.WebApi.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Espresso.WebApi.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DatabaseServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddDatabaseServices(
            this IServiceCollection services,
            IWebApiConfiguration configuration
        )
        {
            services.AddDatabaseConfiguration(configuration);
            services.AddRepositories();

            return services;
        }

        private static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IWebApiConfiguration webApiConfiguration)
        {
            services.AddDbContext<IApplicationDatabaseContext, ApplicationDatabaseContext>(options =>
            {
                options.UseSqlServer(
                    connectionString: webApiConfiguration.DatabaseConfiguration.ConnectionString,
                    sqlServerOptionsAction: sqlServerOptions =>
                    {
                        sqlServerOptions.CommandTimeout(webApiConfiguration.DatabaseConfiguration.CommandTimeoutInSeconds);
                    }
                );
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                switch (webApiConfiguration.AppConfiguration.AppEnvironment)
                {
                    case AppEnvironment.Undefined:
                    case AppEnvironment.Local:
                    case AppEnvironment.Dev:
                    default:
                        options.EnableDetailedErrors();
                        options.EnableSensitiveDataLogging(true);
                        break;
                    case AppEnvironment.Prod:
                        break;
                }
            });

            services.AddScoped<IDatabaseConnectionFactory>(serviceProvider => new DatabaseConnectionFactory(webApiConfiguration.DatabaseConfiguration.ConnectionString));

            return services;
        }


        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IApplicationDownloadRepository, ApplicationDownloadRepository>();
            services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();

            return services;
        }

    }
}
