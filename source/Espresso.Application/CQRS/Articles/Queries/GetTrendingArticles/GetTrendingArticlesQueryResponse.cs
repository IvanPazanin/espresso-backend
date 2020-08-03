﻿using System.Collections.Generic;
using System.Linq;

namespace Espresso.Application.CQRS.Articles.Queries.GetTrendingArticles
{
    public class GetTrendingArticlesQueryResponse
    {
        public IEnumerable<GetTrendingArticlesArticle> Articles { get; } = new List<GetTrendingArticlesArticle>();

        public GetTrendingArticlesQueryResponse(IEnumerable<GetTrendingArticlesArticle> articles)
        {
            Articles = articles;
        }

        public override string ToString()
        {
            return $"{nameof(Articles)}:{Articles.Count()}";
        }
    }
}
