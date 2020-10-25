﻿using System;
using System.Collections.Generic;

namespace Espresso.WebApi.Application.Articles.Queries.GetLatestArticles
{
    public record GetLatestArticlesQueryResponse
    {
        #region Properties
        public IEnumerable<GetLatestArticlesArticle> Articles { get; init; } = new List<GetLatestArticlesArticle>();

        public IEnumerable<GetLatestArticlesArticle> FeaturedArticles { get; init; } = new List<GetLatestArticlesArticle>();

        public IEnumerable<GetLatestArticlesNewsPortal> NewNewsPortals { get; init; } = new List<GetLatestArticlesNewsPortal>();

        public int NewNewsPortalsPosition { get; init; }

        public string LastAddedNewsPortalDate { get; init; } = "";
        #endregion
    }
}
