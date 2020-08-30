﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Espresso.Domain.Enums.RegionEnums;
using Espresso.Domain.Infrastructure;

namespace Espresso.Domain.Entities
{
    public class Region : IEntity<int, Region>
    {
        #region Constants
        public const int RegionNameHasMaxLength = 100;
        public const int RegionSubtitleHasMaxLength = 100;
        #endregion

        #region Properties
        public int Id { get; private set; }

        public string Name { get; private set; }

        public string Subtitle { get; private set; }

        public IEnumerable<NewsPortal> NewsPortals { get; private set; } = new List<NewsPortal>();
        #endregion

        #region Constructors
        public Region(int id, string name, string subtitle)
        {
            Id = id;
            Name = name;
            Subtitle = subtitle;
        }
        #endregion

        #region Methods
        public static Expression<Func<Region, object>> GetOrderByRegionNameExpression()
        {
            return region => region.Name;
        }

        public static Expression<Func<Region, bool>> GetAllRegionsExpectGlobalPredicate()
        {
            return region => !region.Id.Equals((int)RegionId.Global);
        }
        #endregion
    }
}

