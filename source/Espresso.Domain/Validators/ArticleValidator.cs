﻿using System;
using System.Collections.Generic;

using Espresso.Domain.Entities;
using Espresso.Domain.IValidators;

namespace Espresso.Domain.Validators
{
    public class ArticleValidator : Validator, IArticleValidator
    {
        #region Methods
        public Article Validate(
            Guid id,
            string? url,
            string? webUrl,
            string? summary,
            string? title,
            string? imageUrl,
            DateTime createDateTime,
            DateTime updateDateTime,
            DateTime? publishDateTime,
            int numberOfClicks,
            decimal trendingScore,
            int newsPortalId,
            int rssFeedId,
            IEnumerable<ArticleCategory> articleCategories,
            NewsPortal? newsPortal,
            RssFeed rssFeed
        )
        {
            NotEmpty(id, nameof(Article.Id));

            NotEmpty(url!, nameof(Article.Url));
            MaxLength(url!, nameof(Article.Url), Article.UrlMaxLength);
            MustBeUrl(url!, nameof(Article.Url));

            NotEmpty(webUrl!, nameof(Article.WebUrl));
            MaxLength(webUrl!, nameof(Article.WebUrl), Article.WebUrlMaxLength);
            MustBeUrl(webUrl!, nameof(Article.WebUrl));

            NotEmpty(summary!, nameof(Article.Summary));
            MaxLength(summary!, nameof(Article.Summary), Article.SummaryMaxLength);

            NotEmpty(title!, nameof(Article.Title));
            MaxLength(title!, nameof(Article.Title), Article.TitleMaxLength);

            if (imageUrl != null)
            {
                MaxLength(imageUrl, nameof(Article.ImageUrl), Article.ImageUrlMaxLength);
                MustBeUrl(imageUrl, nameof(Article.ImageUrl));
            }

            NotEmpty(publishDateTime, nameof(publishDateTime));

            NotEmpty(newsPortalId, nameof(newsPortalId));

            NotEmpty(rssFeedId, nameof(rssFeedId));

            NotEmpty(articleCategories, nameof(articleCategories));

            NotEmpty(newsPortal, nameof(newsPortal));

            return new Article(
                id: id,
                url: url!,
                webUrl: webUrl!,
                summary: summary!,
                title: title!,
                imageUrl: imageUrl,
                createDateTime: createDateTime,
                updateDateTime: updateDateTime,
                publishDateTime: publishDateTime!.Value,
                numberOfClicks: numberOfClicks,
                trendingScore: trendingScore,
                isHidden: Article.IsHiddenDefaultValue,
                isFeatured: Article.IsFeaturedDefaultValue,
                newsPortalId: newsPortalId,
                rssFeedId: rssFeedId,
                articleCategories: articleCategories,
                newsPortal: newsPortal,
                rssFeed: rssFeed
            );
        }
        #endregion
    }
}
