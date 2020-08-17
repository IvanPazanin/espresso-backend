﻿using System.Collections.Generic;
using System.Linq;

namespace Espresso.Application.CQRS.Configuration.Queries.GetConfiguration
{
    public class GetConfigurationQueryResponse
    {
        public IEnumerable<GetConfigurationCategoryWithNewsPortals> CategoriesWithNewsPortals { get; }

        public IEnumerable<GetConfigurationCategory> Categories { get; }

        public IEnumerable<GetConfigurationRegion> Regions { get; set; }

        public GetConfigurationQueryResponse(
            IEnumerable<GetConfigurationCategoryWithNewsPortals> categoriesWithNewsPortals,
            IEnumerable<GetConfigurationCategory> categories,
            IEnumerable<GetConfigurationRegion> regions
        )
        {
            this.CategoriesWithNewsPortals = categoriesWithNewsPortals;
            Categories = categories;
            Regions = regions;
        }

        public override string ToString()
        {
            return $"{nameof(Categories)}:{Categories.Count()}, " +
                $"{nameof(CategoriesWithNewsPortals)}:{CategoriesWithNewsPortals.Count()}, " +
                $"{nameof(Regions)}:{Regions.Count()}";
        }
    }
}
