﻿// GetLatestArticlesArticle_1_3.cs
//
// © 2022 Espresso News. All rights reserved.

using Espresso.Common.Constants;
using Espresso.Domain.Entities;

namespace Espresso.WebApi.Application.Articles.Queries.GetLatestArticles_1_3;
#pragma warning disable S101 // Types should be named in PascalCase
public record GetLatestArticlesArticle_1_3
#pragma warning restore S101 // Types should be named in PascalCase
{
    /// <summary>
    /// Gets iD created by app.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets article Url provided by RSS Feed.
    /// </summary>
    public string Url { get; private set; } = string.Empty;

    /// <summary>
    /// Gets article Title Parsed from RSS Feed.
    /// </summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// Gets image URL parsed from src attribute of first img element or second rss feed link, first is.
    /// </summary>
    public string? ImageUrl { get; private set; }

    /// <summary>
    /// Gets article Publish time provided by RSS Feed.
    /// </summary>
    public string PublishDateTime { get; private set; } = string.Empty;

    /// <summary>
    /// Gets news Portal ID.
    /// </summary>
    public GetLatestArticlesNewsPortal_1_3? NewsPortal { get; private set; }

    /// <summary>
    /// Gets list Of Categories article belongs to.
    /// </summary>
    public IEnumerable<GetLatestArticlesCategory_1_3> Categories { get; private set; } = [];

    private GetLatestArticlesArticle_1_3()
    {
    }

    public static Expression<Func<Article, GetLatestArticlesArticle_1_3>> GetProjection()
    {
        return article => new GetLatestArticlesArticle_1_3
        {
            Id = article.Id,
            Title = article.Title,
            ImageUrl = article.ImageUrl,
            Url = article.Url,
            PublishDateTime = article.PublishDateTime.ToString(DateTimeConstants.MobileAppDateTimeFormat, CultureInfo.InvariantCulture),
            NewsPortal = GetLatestArticlesNewsPortal_1_3.GetProjection()
                .Compile()
                .Invoke(article.NewsPortal!),
            Categories = article.ArticleCategories
                .AsQueryable()
                .Select(articleCategory => articleCategory.Category)
                .Select(GetLatestArticlesCategory_1_3.GetProjection()!),
        };
    }
}
