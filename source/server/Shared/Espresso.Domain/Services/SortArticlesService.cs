﻿// SortArticlesService.cs
//
// © 2022 Espresso News. All rights reserved.

using Espresso.Domain.Entities;
using Espresso.Domain.IServices;

namespace Espresso.Domain.Services;

public class SortArticlesService : ISortArticlesService
{
    public (
        IEnumerable<Article> createdArticles,
        IEnumerable<(Article article, IEnumerable<string> modifiedProperties)> updatedArticlesWithModifiedProperties,
        IEnumerable<ArticleCategory> createArticleCategories,
        IEnumerable<ArticleCategory> deleteArticleCategories) SortArticles(
        IEnumerable<Article> articles,
        IDictionary<Guid, Article> savedArticles)
    {
        var createArticles = new List<Article>();
        var updatedArticlesWithModifiedProperties = new List<(Article article, IEnumerable<string> modifiedProperties)>();
        var createArticleCategories = new List<ArticleCategory>();
        var deleteArticleCategories = new List<ArticleCategory>();

        var savedArticlesArticleIdDictionary = new Dictionary<(int newsPortalId, string articleUrl), Guid>();
        var savedArticlesTitleDictionary = new Dictionary<(int newsPortalId, string title), Guid>();
        var savedArticlesSummaryDictionary = new Dictionary<(int newsPortalId, string summary), Guid>();

        foreach (var (id, article) in savedArticles)
        {
            _ = savedArticlesArticleIdDictionary.TryAdd((article.NewsPortalId, article.Url), id);
            _ = savedArticlesTitleDictionary.TryAdd((article.NewsPortalId, article.Title), id);
            _ = savedArticlesSummaryDictionary.TryAdd((article.NewsPortalId, article.Summary), id);
        }

        foreach (var article in articles)
        {
            if (
                savedArticlesArticleIdDictionary.TryGetValue((article.NewsPortalId, article.Url), out var savedArticleId) ||
                savedArticlesTitleDictionary.TryGetValue((article.NewsPortalId, article.Title), out savedArticleId) ||
                savedArticlesSummaryDictionary.TryGetValue((article.NewsPortalId, article.Summary), out savedArticleId))
            {
                var savedArticle = savedArticles[savedArticleId];
                var (shouldUpdate, articleCategoriesToCreate, articleCategoriesToDelete, modifiedProperties) = savedArticle.Update(article);

                if (shouldUpdate)
                {
                    createArticleCategories.AddRange(articleCategoriesToCreate);
                    deleteArticleCategories.AddRange(articleCategoriesToDelete);
                    updatedArticlesWithModifiedProperties.Add((savedArticle, modifiedProperties));
                }
            }
            else
            {
                createArticles.Add(article);
            }
        }

        return (createArticles, updatedArticlesWithModifiedProperties, createArticleCategories, deleteArticleCategories);
    }

    public async Task<IEnumerable<Article>> RemoveDuplicateArticles(ChannelReader<Article> articlesChannelReader, CancellationToken cancellationToken)
    {
        var articleIdArticleDictionary = new Dictionary<(int newsPortalId, string articleId), Guid>();
        var titleArticleDictionary = new Dictionary<(int newsPortalId, string title), Guid>();
        var summaryArticleDictionary = new Dictionary<(int newsPortalId, string summary), Guid>();

        var uniqueArticles = new Dictionary<Guid, Article>();

        var articles = articlesChannelReader.ReadAllAsync(cancellationToken);

        await foreach (var article in articles)
        {
            if (
                articleIdArticleDictionary.TryGetValue((article.NewsPortalId, article.Url), out var alreadyParsedArticleId) ||
                titleArticleDictionary.TryGetValue((article.NewsPortalId, article.Title), out alreadyParsedArticleId) ||
                summaryArticleDictionary.TryGetValue((article.NewsPortalId, article.Summary), out alreadyParsedArticleId))
            {
                if (uniqueArticles.TryGetValue(alreadyParsedArticleId, out var parsedArticle))
                {
                    parsedArticle.UpdateArticleCategories(article.ArticleCategories);
                }
            }
            else
            {
                _ = uniqueArticles.TryAdd(article.Id, article);
                _ = articleIdArticleDictionary.TryAdd((article.NewsPortalId, article.Url), article.Id);
                _ = titleArticleDictionary.TryAdd((article.NewsPortalId, article.Title), article.Id);
                _ = summaryArticleDictionary.TryAdd((article.NewsPortalId, article.Summary), article.Id);
            }
        }

        return uniqueArticles.Values;
    }
}
