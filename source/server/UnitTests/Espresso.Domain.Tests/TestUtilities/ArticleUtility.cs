﻿// ArticleUtility.cs
//
// © 2022 Espresso News. All rights reserved.

using Espresso.Domain.Entities;
using Espresso.Domain.ValueObjects.ArticleValueObjects;

namespace Espresso.Domain.Tests.TestUtilities;

/// <summary>
/// ArticleUtility.
/// </summary>
public static class ArticleUtility
{
    public static Article CreateDefaultArticleWith(
        int newsPortalId = default,
        string articleUrl = "",
        string title = "",
        string summary = "",
        DateTimeOffset publishDateTime = default,
        DateTimeOffset createDateTime = default,
        int categoryId = default,
        Guid id = default)
    {
        var category = new Category(
            id: categoryId,
            name: string.Empty,
            color: string.Empty,
            keyWordsRegexPattern: default,
            sortIndex: default,
            position: default,
            categoryType: default,
            categoryUrl: string.Empty);

        var articleCategories = new List<ArticleCategory>()
            {
                new(
                    id: default,
                    article: null,
                    articleId: default,
                    category: category,
                    categoryId: default),
            };

        var newsPortal = new NewsPortal(
            id: newsPortalId,
            name: string.Empty,
            baseUrl: string.Empty,
            iconUrl: string.Empty,
            isNewOverride: null,
            createdAt: default,
            categoryId: default,
            regionId: default,
            isEnabled: true);

        var article = new Article(
            id: id,
            url: articleUrl,
            webUrl: string.Empty,
            summary: summary,
            title: title,
            imageUrl: default,
            createDateTime: createDateTime,
            updateDateTime: default,
            publishDateTime: publishDateTime,
            numberOfClicks: default,
            trendingScore: default,
            editorConfiguration: new EditorConfiguration(),
            newsPortalId: newsPortal.Id,
            rssFeedId: default,
            articleCategories: articleCategories,
            newsPortal: newsPortal,
            rssFeed: default,
            firstSimilarArticles: null,
            secondSimilarArticles: null);

        return article;
    }
}
