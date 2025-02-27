﻿// OrderArticleCollectionExtensions.cs
//
// © 2022 Espresso News. All rights reserved.

using Espresso.Domain.Entities;

namespace Espresso.Domain.Extensions;

public static class OrderArticleCollectionExtensions
{
    public static IOrderedEnumerable<Article> OrderFeaturedArticles(
        this IEnumerable<Article> articles,
        IEnumerable<int>? categoryIds)
    {
        var categoriesWithOrderIndex = categoryIds
            ?.Select((category, index) => (category, index))
            .ToDictionary(
                categoryWithOrderIndex => categoryWithOrderIndex.category,
                categoryWithOrderIndex => categoryWithOrderIndex.index);

        const int HalfOfMaxValue = int.MaxValue / 2;

        var orderedArticles = articles
            .OrderBy(
                article =>
                {
                    if (article.EditorConfiguration.FeaturedPosition is not null)
                    {
                        return article.EditorConfiguration.FeaturedPosition.Value;
                    }

                    if (categoriesWithOrderIndex is null)
                    {
                        return HalfOfMaxValue;
                    }

                    var categoryId = article.ArticleCategories.First().CategoryId;
                    return categoriesWithOrderIndex.TryGetValue(categoryId, out var orderIndex) ? HalfOfMaxValue + orderIndex : int.MaxValue;
                })
            .OrderArticlesByTrendingScore();

        return orderedArticles;
    }

    public static IEnumerable<Article> OrderArticlesByCategory(
        this IEnumerable<Article> articles,
        IEnumerable<int>? categoryIds)
    {
        if (categoryIds is null)
        {
            return articles;
        }

        var categoriesWithOrderIndex = categoryIds
            .Select((category, index) => (category, index))
            .ToDictionary(
                categoryWithOrderIndex => categoryWithOrderIndex.category,
                categoryWithOrderIndex => categoryWithOrderIndex.index);

        const int HalfOfMaxValue = int.MaxValue / 2;

        var orderedArticles = articles
            .OrderBy(
                article =>
                {
                    if (categoriesWithOrderIndex is null)
                    {
                        return HalfOfMaxValue;
                    }

                    var articleCategory = article.ArticleCategories.FirstOrDefault();

                    if (articleCategory is null)
                    {
                        return HalfOfMaxValue;
                    }

                    var categoryId = articleCategory.CategoryId;
                    return categoriesWithOrderIndex.TryGetValue(categoryId, out var orderIndex) ? HalfOfMaxValue + orderIndex : int.MaxValue;
                })
            .OrderArticlesByTrendingScore();

        return orderedArticles;
    }

    public static IOrderedEnumerable<Article> OrderArticlesByPublishDate(
        this IEnumerable<Article> articles)
    {
        var orderedArticles = articles.OrderByDescending(article => article.PublishDateTime);

        return orderedArticles;
    }

    public static IOrderedEnumerable<Article> OrderArticlesByTrendingScore(
        this IEnumerable<Article> articles)
    {
        var orderedArticles = articles.OrderByDescending(article => article.TrendingScore);

        return orderedArticles;
    }

    public static IOrderedEnumerable<Article> OrderArticlesByTrendingScore(
        this IOrderedEnumerable<Article> articles)
    {
        var orderedArticles = articles.ThenByDescending(article => article.TrendingScore);

        return orderedArticles;
    }

    public static IOrderedEnumerable<Article> OrderArticlesByNumberOfClicks(
    this IEnumerable<Article> articles)
    {
        var orderedArticles = articles.OrderByDescending(article => article.NumberOfClicks);

        return orderedArticles;
    }
}
