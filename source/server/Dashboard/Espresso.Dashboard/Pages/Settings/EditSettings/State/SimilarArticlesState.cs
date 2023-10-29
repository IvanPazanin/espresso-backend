// SimilarArticlesState.cs
//
// © 2021 Espresso News. All rights reserved.

namespace Espresso.Dashboard.Pages.Settings.EditSettings.State;

public class SimilarArticlesState
{
    public int MaxDurationBetweenTwoSimilarArticlesInHours { get; set; }

    public int MaxArticleAgeToParseInSimilarArticlesInHours { get; set; }

    public int MinimalNumberOfWordsRequirement { get; set; }

    public double SimilarityScoreThreshold { get; set; }
}
