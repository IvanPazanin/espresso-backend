﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Espresso.Common.Constants;
using Espresso.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Espresso.WebApi.Application.Articles.Queries.GetFeaturedArticles
{
    public class GetFeaturedArticlesQueryHandler :
        IRequestHandler<GetFeaturedArticlesQuery, GetFeaturedArticlesQueryResponse>
    {
        #region Fields
        private readonly IMemoryCache _memoryCache;
        #endregion

        #region Constructors
        public GetFeaturedArticlesQueryHandler(
            IMemoryCache memoryCache
        )
        {
            _memoryCache = memoryCache;
        }
        #endregion

        #region Methods
        public Task<GetFeaturedArticlesQueryResponse> Handle(GetFeaturedArticlesQuery request, CancellationToken cancellationToken)
        {
            var articles = _memoryCache.Get<IEnumerable<Article>>(
                key: MemoryCacheConstants.ArticleKey
            );

            var firstArticle = articles.FirstOrDefault(
                article => article.Id.Equals(request.FirstArticleId)
            );

            var newsPortalIds = request.NewsPortalIds
                ?.Replace(" ", "")
                ?.Split(',')
                ?.Select(newsPortalIdString => int.TryParse(newsPortalIdString, out var newsPortalId) ? newsPortalId : default)
                ?.Where(newsPortalId => newsPortalId != default);

            var categoryIds = request.CategoryIds
                ?.Replace(" ", "")
                ?.Split(',')
                ?.Select(categoryIdString => int.TryParse(categoryIdString, out var categoryId) ? categoryId : default)
                ?.Where(categoryId => categoryId != default);

            var featuredArticleDtos = articles
                .Where(
                    predicate: Article.GetFilteredFeaturedArticlesPredicate(
                        categoryIds: categoryIds,
                        newsPortalIds: newsPortalIds,
                        titleSearchQuery: null,
                        maxAgeOfFeaturedArticle: request.MaxAgeOfFeaturedArticle,
                        articleCreateDateTime: firstArticle?.CreateDateTime
                    ).Compile()
                )
                .OrderByDescending(
                    keySelector: Article
                        .GetOrderByDescendingTrendingScoreExpression()
                        .Compile()
                )
                .ThenByDescending(
                    keySelector: Article
                        .GetOrderByDescendingTrendingScoreExpression()
                        .Compile()
                )
                .Select(GetFeaturedArticlesArticle.GetProjection().Compile());

            var trendingArticleDtos = articles
                .Where(
                    predicate: Article.GetTrendingArticlePredicate(
                        maxAgeOfTrendingArticle: request.MaxAgeOfTrendingArticle,
                        articleCreateDateTime: firstArticle?.CreateDateTime
                    )
                    .Compile()
                )
                .OrderByDescending(
                    keySelector: Article
                        .GetOrderByDescendingTrendingScoreExpression()
                        .Compile()
                    )
                .Select(GetFeaturedArticlesArticle.GetProjection().Compile());

            var articleDtos = featuredArticleDtos
                .Union(trendingArticleDtos)
                .Skip(request.Skip)
                .Take(request.Take);

            var response = new GetFeaturedArticlesQueryResponse
            {
                Articles = articleDtos
            };

            return Task.FromResult(result: response);
        }
        #endregion
    }
}
