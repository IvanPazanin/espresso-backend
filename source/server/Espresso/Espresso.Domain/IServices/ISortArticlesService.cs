﻿using System;
using System.Collections.Generic;
using Espresso.Domain.Entities;

namespace Espresso.Domain.IServices
{
    public interface ISortArticlesService
    {
        public (
            IEnumerable<Article> createdArticles,
            IEnumerable<(Article article, IEnumerable<string> modifiedProperties)> updatedArticlesWithModifiedProperties,
            IEnumerable<ArticleCategory> createArticleCategories,
            IEnumerable<ArticleCategory> deleteArticleCategories
        ) SortArticles(
            IEnumerable<Article> articles,
            IDictionary<Guid, Article> savedArticles
        );

        public IEnumerable<Article> RemoveDuplicateArticles(IEnumerable<Article> articles);
    }
}
