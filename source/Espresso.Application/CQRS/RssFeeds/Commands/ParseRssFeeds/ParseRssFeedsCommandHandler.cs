﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Espresso.Application.DataTransferObjects;
using Espresso.Application.IService;
using Espresso.Application.IServices;
using Espresso.Common.Constants;
using Espresso.Common.Enums;
using Espresso.Common.Utilities;
using Espresso.Domain.Entities;
using Espresso.Domain.Enums.ApplicationDownloadEnums;
using Espresso.Domain.Extensions;
using Espresso.Domain.Records;
using Espresso.Persistence.IRepositories;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Espresso.Application.CQRS.RssFeeds.Commands.ParseRssFeeds
{
    public class ParseRssFeedsCommandHandler : IRequestHandler<ParseRssFeedsCommand>
    {
        #region Fields
        private readonly IMemoryCache _memoryCache;
        private readonly IArticleRepository _articleRepository;
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        private readonly IParseArticlesService _parseArticlesService;
        private readonly ISlackService _slackService;
        private readonly ILoadRssFeedsService _loadRssFeedsService;
        private readonly IHttpService _httpService;
        private readonly ISortArticlesService _sortArticlesService;
        private readonly ILogger<ParseRssFeedsCommandHandler> _logger;
        #endregion

        #region Constructors
        public ParseRssFeedsCommandHandler(
            IMemoryCache memoryCache,
            IArticleRepository articleRepository,
            IArticleCategoryRepository articleCategoryRepository,
            IParseArticlesService parseArticlesService,
            ISlackService slackService,
            ILoggerFactory loggerFactory,
            ILoadRssFeedsService loadRssFeedsService,
            IHttpService httpService,
            ISortArticlesService sortArticlesService
        )
        {
            _memoryCache = memoryCache;
            _articleRepository = articleRepository;
            _articleCategoryRepository = articleCategoryRepository;
            _parseArticlesService = parseArticlesService;
            _slackService = slackService;
            _loadRssFeedsService = loadRssFeedsService;
            _httpService = httpService;
            _sortArticlesService = sortArticlesService;
            _logger = loggerFactory.CreateLogger<ParseRssFeedsCommandHandler>();
        }
        #endregion

        #region Methods

        #region Public Methods
        public async Task<Unit> Handle(
            ParseRssFeedsCommand request,
            CancellationToken cancellationToken
        )
        {
            var categories = _memoryCache.Get<IEnumerable<Category>>(key: MemoryCacheConstants.CategoryKey);

            var rssFeeds = _memoryCache.Get<IEnumerable<RssFeed>>(key: MemoryCacheConstants.RssFeedKey);

            var rssFeedItems = await _loadRssFeedsService.ParseRssFeeds(
                rssFeeds: rssFeeds,
                appEnvironment: request.AppEnvironment,
                currentApiVersion: request.CurrentApiVersion,
                cancellationToken: cancellationToken
            );

            // To Save SkipParseConfiguration.CurrentSkip
            _ = _memoryCache.Set(
                key: MemoryCacheConstants.RssFeedKey,
                value: rssFeeds.ToList()
            );

            var articles = await GetArticlesFromLoadedRssFeeds(
                rssFeedItems: rssFeedItems,
                categories: categories,
                request: request,
                cancellationToken: cancellationToken
            );

            var uniqueArticles = _sortArticlesService.RemoveDuplicateArticles(articles);

            var (createArticles, updateArticles) = _sortArticlesService.SortArticles(
                articles: uniqueArticles,
                savedArticles: _memoryCache.Get<IEnumerable<Article>>(MemoryCacheConstants.ArticleKey)
            );

            CreateArticles(articles: createArticles);
            UpdateArticles(articles: updateArticles);

            await CallWebServer(
                request: request,
                createArticles: createArticles,
                updateArticles: updateArticles,
                cancellationToken: cancellationToken
            );

            return Unit.Value;
        }
        #endregion

        #region Private Methods
        public async Task<IEnumerable<Article>> GetArticlesFromLoadedRssFeeds(
            IEnumerable<RssFeedItem> rssFeedItems,
            IEnumerable<Category> categories,
            ParseRssFeedsCommand request,
            CancellationToken cancellationToken
        )
        {
            var initialCapacity = rssFeedItems.Count();
            var parsedArticles = new ConcurrentDictionary<Guid, Article>();

            var tasks = new List<Task>();
            var random = new Random();

            foreach (var rssFeedItem in rssFeedItems)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var initialNumberOfClicks = random.Next(0, 15);
                    try
                    {
                        var article = await _parseArticlesService.CreateArticleAsync(
                            rssFeedItem: rssFeedItem,
                            categories: categories,
                            maxAgeOfArticle: request.MaxAgeOfArticle,
                            cancellationToken: cancellationToken
                        );

                        _ = parsedArticles.TryAdd(article.Id, article);
                    }
                    catch (Exception exception)
                    {
                        var rssFeedUrl = rssFeedItem.RssFeed.Url;
                        var exceptionMessage = exception.Message;
                        var innerExceptionMessage = exception.InnerException?.Message ?? FormatConstants.EmptyValue;
                        var eventName = Event.ArticleParsing.GetDisplayName();
                        var eventId = (int)Event.ArticleParsing;
                        var message = $"RssFeedUrl: {rssFeedUrl}";

                        if (exception.Message.Equals("articleCategories must not be empty! (Parameter 'articleCategories')"))
                        {
                            var urlCategories = string.Join(
                                separator: ", ",
                                values: rssFeedItem.RssFeed.RssFeedCategories?
                                    .Select(rssFeedCategory => $"{rssFeedCategory.UrlRegex}-{rssFeedCategory.UrlSegmentIndex}:{rssFeedCategory.Category?.Name ?? ""}")
                                    ?? Array.Empty<string>()
                            );
                            await _slackService.LogMissingCategoriesError(
                                version: request.CurrentApiVersion,
                                rssFeedUrl: rssFeedUrl,
                                articleUrl: rssFeedItem.Links?.FirstOrDefault()?.ToString() ?? "",
                                urlCategories: urlCategories,
                                appEnvironment: request.AppEnvironment,
                                cancellationToken: cancellationToken
                            );
                        }
                        else if (exception.Message.Equals("publishDateTime must not be empty! (Parameter 'publishDateTime')"))
                        {

                        }
                        else
                        {
                            _logger.LogWarning(
                                eventId: new EventId(id: eventId, name: eventName),
                                message: $"{AnsiUtility.EncodeEventName("{0}")}\n\t" +
                                    $"{AnsiUtility.EncodeParameterName(nameof(message))}: " +
                                    $"{AnsiUtility.EncodeRequestParameters("{1}")}\n\t" +
                                    $"{AnsiUtility.EncodeParameterName(nameof(exceptionMessage))}: " +
                                    $"{AnsiUtility.EncodeErrorMessage("{2}")}\n\t" +
                                    $"{AnsiUtility.EncodeParameterName(nameof(innerExceptionMessage))}: " +
                                    $"{AnsiUtility.EncodeErrorMessage("{3}")}",
                                args: new object[]
                                {
                                        eventName,
                                        message,
                                        exceptionMessage,
                                        innerExceptionMessage,
                                }
                            );

                            await _slackService.LogWarning(
                                eventName: eventName,
                                version: request.CurrentApiVersion,
                                message: message,
                                exception: exception,
                                appEnvironment: request.AppEnvironment,
                                cancellationToken: cancellationToken
                            );
                        }
                    }
                }, cancellationToken));
            }

            await Task.WhenAll(tasks);

            return parsedArticles.Values;
        }

        private void CreateArticles(
            IEnumerable<Article> articles
        )
        {
            if (!articles.Any())
            {
                return;
            }
            _ = _memoryCache.Set(
                key: MemoryCacheConstants.DeadLockLogKey,
                value: $"Started {nameof(CreateArticles)}"
            );

            var savedArticles = _memoryCache
                .Get<IEnumerable<Article>>(MemoryCacheConstants.ArticleKey)
                .ToDictionary(article => article.Id);

            var articlesToCreate = new List<Article>();
            var articleCategoriesToCreate = new List<ArticleCategory>();

            foreach (var article in articles)
            {
                if (savedArticles.ContainsKey(article.Id))
                {
                    continue;
                }
                savedArticles.Add(article.Id, article);

                articlesToCreate.Add(article);
                articleCategoriesToCreate.AddRange(article.ArticleCategories);
            }

            _articleRepository.InsertArticles(articlesToCreate);
            _articleCategoryRepository.InsertArticleCategories(articleCategoriesToCreate);

            _ = _memoryCache.Set(
                key: MemoryCacheConstants.ArticleKey,
                value: savedArticles.Values.ToList()
            );
            _ = _memoryCache.Set(
                key: MemoryCacheConstants.DeadLockLogKey,
                value: $"Ended {nameof(CreateArticles)}"
            );
        }

        private void UpdateArticles(
            IEnumerable<Article> articles
        )
        {
            if (!articles.Any())
            {
                return;
            }
            _ = _memoryCache.Set(
                key: MemoryCacheConstants.DeadLockLogKey,
                value: $"Started {nameof(UpdateArticles)}"
            );

            var savedArticles = _memoryCache
                .Get<IEnumerable<Article>>(MemoryCacheConstants.ArticleKey)
                .ToDictionary(article => article.Id);

            var articlesToUpdate = new List<Article>();

            foreach (var article in articles)
            {
                if (savedArticles.ContainsKey(article.Id))
                {
                    _ = savedArticles.Remove(article.Id);
                    savedArticles.Add(article.Id, article);
                    articlesToUpdate.Add(article);
                }
            }
            var createArticleCategories = articlesToUpdate
                .SelectMany(article => article.CreateArticleCategories);

            var deleteArticleCategoryIds = articlesToUpdate
                .SelectMany(
                    article => article
                        .DeleteArticleCategories
                        .Select(deleteArticleCategory => deleteArticleCategory.Id)
                );

            _articleRepository.UpdateArticles(articlesToUpdate);
            _articleCategoryRepository.InsertArticleCategories(createArticleCategories);
            _articleCategoryRepository.DeleteArticleCategories(deleteArticleCategoryIds);

            _ = _memoryCache.Set(
                key: MemoryCacheConstants.ArticleKey,
                value: savedArticles.Values.ToList()
            );
            _ = _memoryCache.Set(
                key: MemoryCacheConstants.DeadLockLogKey,
                value: $"Ended {nameof(UpdateArticles)}"
            );
        }

        private async Task CallWebServer(
            ParseRssFeedsCommand request,
            IEnumerable<Article> createArticles,
            IEnumerable<Article> updateArticles,
            CancellationToken cancellationToken
        )
        {
            if (!createArticles.Any() && !updateArticles.Any())
            {
                return;
            }

            var createdArticleDtos = createArticles.Select(ArticleDto.GetProjection().Compile());
            var updatedArticleDtos = updateArticles.Select(ArticleDto.GetProjection().Compile());

            var httpHeaders = new List<(string headerKey, string headerValue)>
            {
                (headerKey: HttpHeaderConstants.ApiKeyHeaderName, headerValue: request.ParserApiKey),
                (headerKey: HttpHeaderConstants.EspressoApiHeaderName, headerValue: request.TargetedApiVersion),
                (headerKey: HttpHeaderConstants.VersionHeaderName, headerValue: request.CurrentApiVersion),
                (headerKey: HttpHeaderConstants.DeviceTypeHeaderName, headerValue: ((int)DeviceType.RssFeedParser).ToString()),
            };

            try
            {
                await _httpService.PostJsonAsync(
                    url: $"{request.ServerUrl}/api/notifications/articles",
                    data: new ArticlesRequestObjectDto
                    {
                        CreatedArticles = createdArticleDtos,
                        UpdatedArticles = updatedArticleDtos
                    },
                    httpHeaders: httpHeaders,
                    httpClientTimeout: TimeSpan.FromSeconds(30),
                    cancellationToken: cancellationToken
                );

                return;
            }
            catch (Exception exception)
            {
                var eventName = Event.ParserDeleterNewArticlesRequest.GetDisplayName();
                var eventId = (int)Event.ParserDeleterNewArticlesRequest;
                var version = request.CurrentApiVersion;
                var exceptionMessage = exception.Message;
                var innerExceptionMessage = exception.InnerException?.Message ?? FormatConstants.EmptyValue;
                _logger.LogError(
                    eventId: new EventId(
                        id: eventId,
                        name: eventName
                    ),
                    exception: exception,
                    message: $"{AnsiUtility.EncodeEventName("{0}")}\n\t" +
                        $"{AnsiUtility.EncodeParameterName(nameof(version))}: " +
                        $"{AnsiUtility.EncodeVersion("{1}")}\n\t" +
                        $"{AnsiUtility.EncodeParameterName(nameof(exceptionMessage))}: " +
                        $"{AnsiUtility.EncodeErrorMessage("{2}")}\n\t" +
                        $"{AnsiUtility.EncodeParameterName(nameof(innerExceptionMessage))}: " +
                        $"{AnsiUtility.EncodeErrorMessage("{3}")}",
                    args: new object[]
                    {
                            eventName,
                            version,
                            exceptionMessage,
                            innerExceptionMessage,
                    }
                );
                await _slackService.LogError(
                        eventName: eventName,
                        version: request.TargetedApiVersion,
                        message: exception.Message,
                        exception: exception,
                        appEnvironment: request.AppEnvironment,
                        cancellationToken: default
                );
                await Task.Delay(request.WaitDurationAfterWebServerRequestError, cancellationToken);
            }
        }
        #endregion

        #endregion
    }
}

