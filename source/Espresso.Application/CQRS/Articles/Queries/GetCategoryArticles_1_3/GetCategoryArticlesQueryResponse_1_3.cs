﻿using System.Collections.Generic;
using System.Linq;

namespace Espresso.Application.CQRS.Articles.Queries.GetCategoryArticles_1_3
{
    public class GetCategoryArticlesQueryResponse_1_3
    {
        public IEnumerable<GetCategoryArticlesArticle_1_3> Articles { get; } = new List<GetCategoryArticlesArticle_1_3>();

        public GetCategoryArticlesQueryResponse_1_3(
            IEnumerable<GetCategoryArticlesArticle_1_3> articles
        )
        {
            Articles = articles;
        }

        public override string ToString()
        {
            return $"{nameof(Articles)}:{Articles.Count()}";
        }
    }
}
