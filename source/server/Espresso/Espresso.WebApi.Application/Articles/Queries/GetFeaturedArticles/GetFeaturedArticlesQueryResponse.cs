﻿// GetFeaturedArticlesQueryResponse.cs
//
// © 2021 Espresso News. All rights reserved.

using System.Collections.Generic;

namespace Espresso.WebApi.Application.Articles.Queries.GetFeaturedArticles
{
    public record GetFeaturedArticlesQueryResponse
    {
        public IEnumerable<GetFeaturedArticlesArticle> Articles { get; init; } = new List<GetFeaturedArticlesArticle>();
    }
}
