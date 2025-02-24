﻿// HttpClientConstants.cs
//
// © 2022 Espresso News. All rights reserved.

namespace Espresso.Common.Constants;

/// <summary>
/// HttpClientConstants.
/// </summary>
public static class HttpClientConstants
{
    /// <summary>
    /// Slack HTTP client.
    /// </summary>
    public const string SlackHttpClientName = nameof(SlackHttpClientName);

    /// <summary>
    /// Parser to Web API HTTP client.
    /// </summary>
    public const string SendArticlesHttpClientName = nameof(SendArticlesHttpClientName);

    /// <summary>
    /// HTTP client for loading RSS Feeds.
    /// </summary>
    public const string LoadRssFeedsHttpClientName = nameof(LoadRssFeedsHttpClientName);

    /// <summary>
    /// HTTP client for scraping web.
    /// </summary>
    public const string ScrapeWebHttpClientName = nameof(ScrapeWebHttpClientName);
}
