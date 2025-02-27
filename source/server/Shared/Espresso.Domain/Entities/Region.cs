﻿// Region.cs
//
// © 2022 Espresso News. All rights reserved.

using System.Linq.Expressions;
using Espresso.Domain.Enums.RegionEnums;

#pragma warning disable RCS1170

namespace Espresso.Domain.Entities;

public class Region
{
    public const int RegionNameHasMaxLength = 100;
    public const int RegionSubtitleHasMaxLength = 100;

    public int Id { get; private set; }

    public string Name { get; private set; }

    public string Subtitle { get; private set; }

    public ICollection<NewsPortal> NewsPortals { get; private set; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="Region"/> class.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="subtitle"></param>
#pragma warning disable SA1201 // Elements should appear in the correct order
    public Region(int id, string name, string subtitle)
#pragma warning restore SA1201 // Elements should appear in the correct order
    {
        Id = id;
        Name = name;
        Subtitle = subtitle;
    }

    public static Expression<Func<Region, object>> GetOrderByRegionNameExpression()
    {
        return region => region.Name;
    }

    public static Expression<Func<Region, bool>> GetAllRegionsExpectGlobalPredicate()
    {
        return region => !region.Id.Equals((int)RegionId.Global);
    }
}
