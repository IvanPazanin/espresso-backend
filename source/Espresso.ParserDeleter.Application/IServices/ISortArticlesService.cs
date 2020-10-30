﻿using System.Collections.Generic;
using Espresso.Domain.Entities;

namespace Espresso.ParserDeleter.Application.IServices
{
    public interface ISortArticlesService
    {
        public (
            IEnumerable<Article> createdArticles,
            IEnumerable<Article> updatedArticles,
            IEnumerable<ArticleCategory> articleCategoriesToCreate,
            IEnumerable<ArticleCategory> articleCategoriesToDelete
        ) SortArticles(
            IEnumerable<Article> articles,
            IEnumerable<Article> savedArticles
        );

        public IEnumerable<Article> RemoveDuplicateArticles(IEnumerable<Article> articles);
    }
}