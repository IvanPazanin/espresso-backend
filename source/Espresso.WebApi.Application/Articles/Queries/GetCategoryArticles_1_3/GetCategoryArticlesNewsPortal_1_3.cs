﻿using System;
using System.Linq.Expressions;
using Espresso.Domain.Entities;

namespace Espresso.WebApi.Application.Articles.Queries.GetCategoryArticles_1_3
{
    public record GetCategoryArticlesNewsPortal_1_3
    {
        #region Properties
        /// <summary>
        /// News Portal ID
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// News Portal Name
        /// </summary>
        public string Name { get; private set; } = "";

        public string IconUrl { get; private set; } = "";
        #endregion

        #region Constructors
        private GetCategoryArticlesNewsPortal_1_3()
        {
        }
        #endregion

        #region Methods
        public static Expression<Func<NewsPortal, GetCategoryArticlesNewsPortal_1_3>> GetProjection()
        {
            return newsPortal => new GetCategoryArticlesNewsPortal_1_3
            {
                Id = newsPortal.Id,
                Name = newsPortal.Name,
                IconUrl = newsPortal.IconUrl
            };
        }
        #endregion
    }
}
