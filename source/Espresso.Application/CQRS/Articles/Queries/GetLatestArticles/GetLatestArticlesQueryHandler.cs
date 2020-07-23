﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Espresso.Application.ViewModels.ArticleViewModels;
using Espresso.Application.ViewModels.NewsPortalViewModels;
using Espresso.Common.Constants;
using Espresso.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Espresso.Application.CQRS.Articles.Queries.GetLatestArticles
{
    public class GetArticlesQueryHandler : IRequestHandler<GetLatestArticlesQuery, GetLatestArticlesQueryResponse>
    {
        #region Fields
        private readonly IMemoryCache _memoryCache;
        #endregion

        #region Constructors
        public GetArticlesQueryHandler(
            IMemoryCache memoryCache
        )
        {
            _memoryCache = memoryCache;
        }
        #endregion

        #region Methods
        public Task<GetLatestArticlesQueryResponse> Handle(GetLatestArticlesQuery request, CancellationToken cancellationToken)
        {
            var articles = _memoryCache.Get<IEnumerable<Article>>(
                key: MemoryCacheConstants.ArticleKey
            );

            var articleDtos = articles
                .OrderByDescending(keySelector: Article.GetArticleOrderByDescendingExpression().Compile())
                .Where(
                    predicate: Article.GetLatestArticlePredicate(
                        categoryIds: request.CategoryIds,
                        newsPortalIds: request.NewsPortalIds
                    ).Compile()
                )
                .Skip(request.Skip)
                .Take(request.Take)
                .Select(ArticleViewModel.GetProjection().Compile());

            var newsPortals = _memoryCache.Get<IEnumerable<NewsPortal>>(
                key: MemoryCacheConstants.NewsPortalKey
            );

            var newsPortalDtos = newsPortals
                .Where(
                    NewsPortal.GetIsNewExpression(
                        newsPortalIds: request.NewsPortalIds,
                        categoryIds: request.CategoryIds
                    )
                    .Compile()
                )
                .OrderBy(keySelector: NewsPortal.GetOrderByExpression().Compile())
                .Select(selector: NewsPortalViewModel.GetProjection().Compile());

            var response = new GetLatestArticlesQueryResponse(
                articles: articleDtos,
                newNewsPortals: newsPortalDtos,
                newNewsPortalsPosition: DefaultValueConstants.NewNewsPortalsPosition
            );

            return Task.FromResult(result: response);
        }
        #endregion
    }
}
