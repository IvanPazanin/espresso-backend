using System;
using System.Collections.Generic;
using Espresso.Domain.Entities;

namespace Espresso.Domain.Records
{
    public class ArticleData
    {
        public Guid Id { get; set; }

        public string? Url { get; set; }

        public string? WebUrl { get; set; }

        public string? Summary { get; set; }

        public string? Title { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime UpdateDateTime { get; set; }

        public DateTime? PublishDateTime { get; set; }

        public int NumberOfClicks { get; set; }

        public decimal TrendingScore { get; set; }

        public bool IsHidden { get; set; }

        public bool IsFeatured { get; set; }

        public IEnumerable<ArticleCategory>? ArticleCategories { get; set; }
    }
}