﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Espresso.Common.Constants;
using Espresso.Domain.Entities;
using Espresso.Domain.Enums.ApplicationDownloadEnums;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Espresso.Application.CQRS.Configuration.Queries.GetConfiguration
{
    public class GetConfigurationQueryHandler : IRequestHandler<GetConfigurationQuery, GetConfigurationQueryResponse>
    {
        #region Fields
        private readonly IMemoryCache _memoryCache;
        #endregion

        #region Constructors
        public GetConfigurationQueryHandler(
            IMemoryCache memoryCache
        )
        {
            _memoryCache = memoryCache;
        }
        #endregion

        #region Methods
        public Task<GetConfigurationQueryResponse> Handle(GetConfigurationQuery request, CancellationToken cancellationToken)
        {
            var newsPortals = _memoryCache.Get<IEnumerable<NewsPortal>>(key: MemoryCacheConstants.NewsPortalKey);
            var categories = _memoryCache.Get<IEnumerable<Category>>(key: MemoryCacheConstants.CategoryKey);
            var regions = _memoryCache.Get<IEnumerable<Region>>(key: MemoryCacheConstants.RegionKey);

            var newsPortalDtos = newsPortals
                .OrderBy(keySelector: newsPortal => newsPortal.Name)
                .Select(
                    selector: GetConfigurationNewsPortal
                        .GetProjection(
                            maxAgeOfNewNewsPortal: request.MaxAgeOfNewNewsPortal
                        )
                        .Compile()
                );

            var categoryDtos = categories
                .Where(predicate: Category.GetAllCategoriesExceptGeneralExpression().Compile())
                .Select(selector: GetConfigurationCategory.GetProjection().Compile());

            if (request.DeviceType.Equals(DeviceType.WebApp))
            {
                var categoryDtosList = categoryDtos.ToList();
                var sveVijesticategoryDto = new GetConfigurationCategory(
                    id: -1,
                    name: "Sve Vijesti",
                    color: "",
                    position: null,
                    categoryType: CategoryType.General,
                    url: "/"
                );

                categoryDtosList.Insert(0, sveVijesticategoryDto);
                categoryDtos = categoryDtosList;
            }

            var categoriesWithNewsPortals = categories
                .OrderBy(category => category.SortIndex)
                .Where(predicate: Category.GetAllCategoriesExceptLocalExpression().Compile())
                .Select(
                    selector: GetConfigurationCategoryWithNewsPortals
                        .GetProjection(maxAgeOfNewNewsPortal: request.MaxAgeOfNewNewsPortal)
                        .Compile()
                )
                .Where(grouping => grouping.NewsPortals.Count() != 0);

            var regionGroupedNewsPortals = regions
                .OrderBy(keySelector: Region.GetOrderByRegionNameExpression().Compile())
                .Where(predicate: Region.GetAllRegionsExpectGlobalPredicate().Compile())
                .Select(
                    selector: GetConfigurationRegion
                        .GetProjection(
                            maxAgeOfNewNewsPortal: request.MaxAgeOfNewNewsPortal
                        )
                        .Compile()
                );

            var response = new GetConfigurationQueryResponse(
                categoriesWithNewsPortals: categoriesWithNewsPortals,
                categories: categoryDtos,
                regions: regionGroupedNewsPortals
            );

            return Task.FromResult(result: response);
        }
        #endregion
    }
}
