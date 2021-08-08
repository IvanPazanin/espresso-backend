﻿// GetFeaturedArticlesArticleType.cs
//
// © 2021 Espresso News. All rights reserved.

using Espresso.WebApi.Application.Articles.Queries.GetFeaturedArticles;
using GraphQL.Types;

namespace Espresso.WebApi.GraphQl.ApplicationTypes.ArticleTypes.GetFeaturedArticlesTypes
{
    /// <summary>
    /// 
    /// </summary>
    public class GetFeaturedArticlesArticleType : ObjectGraphType<GetFeaturedArticlesArticle>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetFeaturedArticlesArticleType"/> class.
        /// </summary>
        public GetFeaturedArticlesArticleType()
        {
            Name = nameof(GetFeaturedArticlesArticle);
            Field<NonNullGraphType<IdGraphType>>(
                name: nameof(GetFeaturedArticlesArticle.Id)
            );
            Field<NonNullGraphType<StringGraphType>>(
                name: nameof(GetFeaturedArticlesArticle.Title)
            );
            Field<NonNullGraphType<StringGraphType>>(
                name: nameof(GetFeaturedArticlesArticle.Url)
            );
            Field<NonNullGraphType<StringGraphType>>(
                name: nameof(GetFeaturedArticlesArticle.WebUrl)
            );
            Field<StringGraphType>(
                name: nameof(GetFeaturedArticlesArticle.ImageUrl)
            );
            Field<NonNullGraphType<StringGraphType>>(
                name: nameof(GetFeaturedArticlesArticle.PublishDateTime)
            );
            Field<NonNullGraphType<GetFeaturedArticlesNewsPortalType>>(
                name: nameof(GetFeaturedArticlesArticle.NewsPortal)
            );
            Field<NonNullGraphType<ListGraphType<NonNullGraphType<GetFeaturedArticlesCategoryType>>>>(
                name: nameof(GetFeaturedArticlesArticle.Categories)
            );
        }
    }
}
