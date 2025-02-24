﻿// ArticleCategoryConfiguration.cs
//
// © 2022 Espresso News. All rights reserved.

using Espresso.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Espresso.Persistence.Configuration;

/// <summary>
/// <see cref="ArticleCategory"/> entity configuration.
/// </summary>
public class ArticleCategoryConfiguration : IEntityTypeConfiguration<ArticleCategory>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ArticleCategory> builder)
    {
        _ = builder.HasOne(articleCategory => articleCategory.Article)
            .WithMany(article => article!.ArticleCategories)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(articleCategory => articleCategory.ArticleId);

        _ = builder.HasOne(articleCategory => articleCategory.Category)
            .WithMany(category => category!.ArticleCategories)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(articleCategory => articleCategory.CategoryId);
    }
}
