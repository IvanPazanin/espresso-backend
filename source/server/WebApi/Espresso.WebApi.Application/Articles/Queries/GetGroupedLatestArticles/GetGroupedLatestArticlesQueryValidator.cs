﻿// GetGroupedLatestArticlesQueryValidator.cs
//
// © 2022 Espresso News. All rights reserved.

using FluentValidation;

namespace Espresso.WebApi.Application.Articles.Queries.GetGroupedLatestArticles;

public class GetGroupedLatestArticlesQueryValidator : AbstractValidator<GetGroupedLatestArticlesQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetGroupedLatestArticlesQueryValidator"/> class.
    /// </summary>
    public GetGroupedLatestArticlesQueryValidator()
    {
        _ = RuleFor(query => query.Take).GreaterThan(0).LessThan(100);
        _ = RuleFor(query => query.Skip).GreaterThanOrEqualTo(0);
        _ = RuleForEach(query => query.CategoryIds).Must(categoryId => categoryId != default);
        _ = RuleForEach(query => query.NewsPortalIds).Must(newsPortalId => newsPortalId != default);
    }
}
