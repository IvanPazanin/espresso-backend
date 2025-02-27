﻿// ParseHtmlService.cs
//
// © 2022 Espresso News. All rights reserved.

using Espresso.Common.Extensions;
using Espresso.Dashboard.Application.IServices;
using Espresso.Domain.Entities;
using HtmlAgilityPack;

namespace Espresso.Dashboard.Application.Services;

public partial class ParseHtmlService : IParseHtmlService
{
    public string? GetSrcAttributeFromFirstImgElement(string? html)
    {
        if (string.IsNullOrEmpty(html))
        {
            return null;
        }

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        var imgTag = htmlDocument.DocumentNode.SelectSingleNode("//img");

        var srcAttributeValue = imgTag?.GetAttributeValue("src", null);

        return srcAttributeValue;
    }

    public string? GetText(string? html)
    {
        if (html is null)
        {
            return null;
        }

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        var nodes = htmlDocument.DocumentNode
            .SelectNodes(".//text()")
            ?.Select(node => node?.InnerText) ?? [];

        var summary = HtmlEntity.DeEntitize(string.Join(" ", nodes));
        summary = RemoveInvalidCharactersRegex().Replace(summary, " ").RemoveExtraWhiteSpaceCharacters();

        if (string.IsNullOrWhiteSpace(summary))
        {
            return null;
        }

        if (summary.Length > Article.SummaryMaxLength)
        {
            summary = summary.Replace(@"\n", " ", StringComparison.InvariantCulture);
            summary = $"{string.Concat(summary.Take(Article.SummaryMaxLength - 4))}...";
        }

        return summary;
    }

    public string? GetImageUrlFromAttribute(
        HtmlNodeCollection elementTags,
        string attributeName)
    {
        var imageUrls = elementTags.Select(imgTag => imgTag?.GetAttributeValue(attributeName, null));
        var imageUrl = imageUrls.FirstOrDefault();

        return imageUrl;
    }

    [GeneratedRegex("\\r\\n?|\\n")]
    private static partial Regex RemoveInvalidCharactersRegex();
}
