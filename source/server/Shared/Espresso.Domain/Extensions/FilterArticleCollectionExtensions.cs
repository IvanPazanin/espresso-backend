﻿// FilterArticleCollectionExtensions.cs
//
// © 2022 Espresso News. All rights reserved.

using Espresso.Common.Enums;
using Espresso.Domain.Entities;
using Espresso.Domain.Enums.CategoryEnums;
using Espresso.Domain.Utilities;

namespace Espresso.Domain.Extensions;

public static class FilterArticleCollectionExtensions
{
    public static IEnumerable<string> BannedKeywords =>
            [
                "virus",
                "pandemij",
                "koron",
                "coron",
                "capak",
                "božinović",
                "bozinovic",
                "markotić",
                "markotic",
                "mask",
                "karanten",
                "umrlih",
                "zaraz",
                "zaraž",
                "staracki dom",
                "starački dom",
                "bolnica",
                "prva pomoc",
                "prva pomoć",
                "hitna",
                "zabran",
                "pozitiv",
                "testir",
                "slučaj",
                "slucaj",
                "covid",
                "stozer",
                "stožer",
                "respirator",
                "bolest",
                "ambulant",
            ];

    public static IEnumerable<Article> FilterArticlesWithCoronaVirusContentForIosRelease(
        this IEnumerable<Article> articles,
        DeviceType deviceType,
        string targetedApiVersion)
    {
        if (!(deviceType == DeviceType.Ios && targetedApiVersion == "2.0"))
        {
            return articles;
        }

        var filteredArticles = articles.Where(
            article => !BannedKeywords.Any(
                bannedKeyword => article.Title.Contains(bannedKeyword, StringComparison.OrdinalIgnoreCase) ||
                    article.Summary.Contains(bannedKeyword, StringComparison.OrdinalIgnoreCase)));

        return filteredArticles;
    }

    public static IEnumerable<Article> FilterArticlesContainingKeyWords(this IEnumerable<Article> articles, IEnumerable<string> keywords)
    {
        return articles
            .Where(article => !keywords.Any(keyword => article.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase)));
    }

    public static IEnumerable<Article> FilterArticles(
        this IEnumerable<Article> articles,
        IEnumerable<int>? categoryIds,
        IEnumerable<int>? newsPortalIds,
        string? titleSearchTerm,
        DateTimeOffset? articleCreateDateTime)
    {
        var articleMinimumAge = articleCreateDateTime ?? DateTimeOffset.UtcNow;
        var searchTerms = LanguageUtility.GetSearchTerms(titleSearchTerm);

        var filteredArticles = articles
            .Where(
                article =>
                    !article.EditorConfiguration.IsHidden &&
                    article.CreateDateTime <= articleMinimumAge &&
                    (categoryIds == null || article
                        .ArticleCategories
                        .Any(articleCategory => categoryIds.Contains(articleCategory.CategoryId))) &&
                    (newsPortalIds?.Contains(article.NewsPortalId) != false) &&
                    (
                        searchTerms?
                            .All(searchTerm => article
                                .Title
                                .ReplaceCroatianCharacters()
                                .Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) != false));

        return filteredArticles;
    }

    public static IEnumerable<Article> FilterArticles(
        this IEnumerable<Article> articles,
        int categoryId,
        IEnumerable<int>? newsPortalIds,
        string? titleSearchTerm,
        DateTimeOffset? articleCreateDateTime)
    {
        var categoryIds = new[] { categoryId };

        var filteredArticles = articles.FilterArticles(
            categoryIds: categoryIds,
            newsPortalIds: newsPortalIds,
            titleSearchTerm: titleSearchTerm,
            articleCreateDateTime: articleCreateDateTime);

        return filteredArticles;
    }

    public static IEnumerable<Article> FilterFeaturedArticles(
        this IEnumerable<Article> articles,
        IEnumerable<int>? categoryIds,
        IEnumerable<int>? newsPortalIds,
        TimeSpan maxAgeOfFeaturedArticle,
        DateTimeOffset? articleCreateDateTime)
    {
        var articleMinimumAge = articleCreateDateTime ?? DateTimeOffset.UtcNow;
        var maxDateTimeOfFeaturedArticle = DateTimeOffset.UtcNow - maxAgeOfFeaturedArticle;

        var filteredArticles = articles
            .Where(
                article =>
                    article.EditorConfiguration?.IsHidden == false &&
                    article.EditorConfiguration?.IsFeatured == true &&
                    article.CreateDateTime <= articleMinimumAge &&
                    article.PublishDateTime > maxDateTimeOfFeaturedArticle &&
                    (categoryIds == null || article
                        .ArticleCategories
                        .Any(articleCategory => categoryIds.Contains(articleCategory.CategoryId))) &&
                    (newsPortalIds?.Contains(article.NewsPortalId) != false));

        return filteredArticles;
    }

    public static IEnumerable<Article> FilterTrendingArticles(
        this IEnumerable<Article> articles,
        TimeSpan maxAgeOfTrendingArticle,
        DateTimeOffset? articleCreateDateTime,
        int? categoryId)
    {
        var maxTrendingDateTime = DateTimeOffset.UtcNow - maxAgeOfTrendingArticle;

        var articleMinimumAge = articleCreateDateTime ?? DateTimeOffset.UtcNow;

        var filteredArticles = articles
            .Where(
                article =>
                {
                    var isArticleHidden = article.EditorConfiguration?.IsHidden == false;
                    if (!isArticleHidden)
                    {
                        return false;
                    }

                    var isArticleFeatured = article.EditorConfiguration?.IsFeatured != false;
                    if (!isArticleFeatured)
                    {
                        return false;
                    }

                    var isArticleOldEnough = article.CreateDateTime <= articleMinimumAge;
                    if (!isArticleOldEnough)
                    {
                        return false;
                    }

                    var isArticlePublished = article.PublishDateTime > maxTrendingDateTime;
                    if (!isArticlePublished)
                    {
                        return false;
                    }

                    var isCategoryIdSpecified = categoryId is not null;

                    if (isCategoryIdSpecified)
                    {
                        return article.ArticleCategories.Any(articleCategory => articleCategory.CategoryId == categoryId);
                    }

                    var isArticleLocal = !article.ArticleCategories.Any(articleCategory => articleCategory.Category!.CategoryType == CategoryType.Local);

                    if (!isArticleLocal)
                    {
                        return false;
                    }

                    return true;
                });

        return filteredArticles;
    }

    public static IEnumerable<IEnumerable<Article>> GetGroupedArticlesBySimilarity(
        this IEnumerable<Article> savedArticles,
        DateTimeOffset? firstArticleCreateDateTime,
        IEnumerable<int>? categoryIds,
        IEnumerable<int>? newsPortalIds,
        IEnumerable<string> keyWordsToFilterOut,
        string? titleSearchTerm)
    {
        var filteredArticlesDictionary = savedArticles
            .OrderArticlesByPublishDate()
            .FilterArticles(
                categoryIds: categoryIds,
                newsPortalIds: newsPortalIds,
                titleSearchTerm: titleSearchTerm,
                articleCreateDateTime: firstArticleCreateDateTime)
            .FilterArticlesContainingKeyWords(keyWordsToFilterOut)
            .ToDictionary(article => article.Id);

        var groupedArticles = new List<IEnumerable<Article>>();

        foreach (var article in filteredArticlesDictionary.Values)
        {
            if (!filteredArticlesDictionary.ContainsKey(article.Id))
            {
                continue;
            }

            var articles = new List<Article> { article };

            var firstArticleIds = article.FirstSimilarArticles.Select(firstSimilarArticle => firstSimilarArticle.FirstArticleId);
            foreach (var firstArticleId in firstArticleIds)
            {
                if (!filteredArticlesDictionary.TryGetValue(firstArticleId, out var firstArticle))
                {
                    continue;
                }

                articles.Add(firstArticle);
                _ = filteredArticlesDictionary.Remove(firstArticleId);
            }

            var secondArticleIds = article.SecondSimilarArticles.Select(secondSimilarArticle => secondSimilarArticle.SecondArticleId);
            foreach (var secondArticleId in secondArticleIds)
            {
                if (!filteredArticlesDictionary.TryGetValue(secondArticleId, out var secondArticle))
                {
                    continue;
                }

                articles.Add(secondArticle);
                _ = filteredArticlesDictionary.Remove(secondArticleId);
            }

            groupedArticles.Add(articles);
        }

        return groupedArticles;
    }
}
