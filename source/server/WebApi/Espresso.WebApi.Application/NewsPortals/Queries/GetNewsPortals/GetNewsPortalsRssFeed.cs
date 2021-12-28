﻿// GetNewsPortalsRssFeed.cs
//
// © 2021 Espresso News. All rights reserved.

using System.Linq.Expressions;
using Espresso.Domain.Entities;

namespace Espresso.Application.NewsPortals;

public class GetNewsPortalsRssFeed
{
    private GetNewsPortalsRssFeed()
    {
    }

    public int Id { get; private set; }

    public string Url { get; private set; } = string.Empty;

    public IEnumerable<GetNewsPortalsRssFeedContentModifier> RssFeedContentModifiers { get; private set; } = new List<GetNewsPortalsRssFeedContentModifier>();

    public IEnumerable<GetNewsPortalsRssFeedCategory> RssFeedCategories { get; private set; } = new List<GetNewsPortalsRssFeedCategory>();

    public static Expression<Func<RssFeed, GetNewsPortalsRssFeed>> GetProjection()
    {
        var rssFeedContentModifierProjection = GetNewsPortalsRssFeedContentModifier.GetProjection();
        var rssFeedCategoryProjection = GetNewsPortalsRssFeedCategory.GetProjection();
        return rssFeed => new GetNewsPortalsRssFeed
        {
            Id = rssFeed.Id,
            Url = rssFeed.Url,
            RssFeedContentModifiers = rssFeed.RssFeedContentModifiers
                .AsQueryable()
                .Select(rssFeedContentModifierProjection),
            RssFeedCategories = rssFeed.RssFeedCategories
                .AsQueryable()
                .Select(rssFeedCategoryProjection),
        };
    }
}
