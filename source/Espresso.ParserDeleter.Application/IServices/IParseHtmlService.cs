﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Espresso.ParserDeleter.Application.IServices
{
    public interface IParseHtmlService
    {
        public string? GetSrcAttributeFromFirstImgElement(string? html);

        public string? GetText(string? html);

        public Task<string?> GetImageUrlFromJsonObjectFromScriptTag(
            HtmlNodeCollection elementTags,
            IEnumerable<string> propertyNames,
            CancellationToken cancellationToken
        );

        public string? GetImageUrlFromSrcAttribute(HtmlNodeCollection elementTags);
    }
}