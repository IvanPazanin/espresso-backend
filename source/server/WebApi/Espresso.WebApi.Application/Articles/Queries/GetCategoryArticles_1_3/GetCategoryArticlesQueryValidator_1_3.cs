﻿// GetCategoryArticlesQueryValidator_1_3.cs
//
// © 2022 Espresso News. All rights reserved.

using FluentValidation;

namespace Espresso.WebApi.Application.Articles.Queries.GetCategoryArticles_1_3;
#pragma warning disable S101 // Types should be named in PascalCase
public class GetcategoryArticlesQueryValidator_1_3 : AbstractValidator<GetCategoryArticlesQuery_1_3>
#pragma warning restore S101 // Types should be named in PascalCase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetcategoryArticlesQueryValidator_1_3"/> class.
    /// </summary>
    public GetcategoryArticlesQueryValidator_1_3()
    {
        _ = RuleFor(query => query.Take).GreaterThan(0).LessThan(100);
        _ = RuleFor(query => query.Skip).GreaterThanOrEqualTo(0);
        _ = RuleFor(query => query.CategoryId).NotEmpty();
        _ = RuleForEach(query => query.NewsPortalIds).Must(newsPortalId => newsPortalId != 0);
    }
}
