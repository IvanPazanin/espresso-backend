﻿// HideArticleCommandHandler.cs
//
// © 2022 Espresso News. All rights reserved.

using Espresso.Common.Constants;
using Espresso.Domain.Entities;
using Espresso.Persistence.Database;
using Espresso.WebApi.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Espresso.WebApi.Application.Articles.Commands.HideArticle;

public class HideArticleCommandHandler : IRequestHandler<HideArticleCommand>
{
    private readonly IEspressoDatabaseContext _espressoDatabaseContext;
    private readonly IMemoryCache _memoryCache;

    /// <summary>
    /// Initializes a new instance of the <see cref="HideArticleCommandHandler"/> class.
    /// </summary>
    /// <param name="espressoDatabaseContext"></param>
    /// <param name="memoryCache"></param>
    public HideArticleCommandHandler(
        IEspressoDatabaseContext espressoDatabaseContext,
        IMemoryCache memoryCache)
    {
        _espressoDatabaseContext = espressoDatabaseContext;
        _memoryCache = memoryCache;
    }

    public async Task Handle(
        HideArticleCommand request,
        CancellationToken cancellationToken)
    {
        var memoryCacheArticles = _memoryCache
            .Get<IEnumerable<Article>>(key: MemoryCacheConstants.ArticleKey)
            !.ToDictionary(article => article.Id);

        var databaseArticle = await _espressoDatabaseContext.Articles.FindAsync(
            keyValues: [request.ArticleId],
            cancellationToken: cancellationToken) ?? throw new NotFoundException(
                typeName: nameof(Article),
                id: request.ArticleId.ToString());

        databaseArticle.SetIsHidden(request.IsHidden);
        _ = _espressoDatabaseContext.Articles.Update(databaseArticle);
        _ = await _espressoDatabaseContext.SaveChangesAsync(cancellationToken);

        if (memoryCacheArticles.TryGetValue(key: request.ArticleId, value: out var memoryCacheArticle))
        {
            memoryCacheArticle.SetIsHidden(request.IsHidden);
            _ = _memoryCache.Set(
                key: MemoryCacheConstants.ArticleKey,
                value: memoryCacheArticles.Values.ToList());
        }
    }
}
