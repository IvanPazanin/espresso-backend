﻿// DeleteOldArticlesCommand.cs
//
// © 2021 Espresso News. All rights reserved.

using Espresso.Application.Infrastructure.MediatorInfrastructure;
using Espresso.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Espresso.Dashboard.Application.DeleteOldArticles
{
    public record DeleteOldArticlesCommand : Request<DeleteOldArticlesCommandResponse>
    {
        public IDictionary<Guid, Article> Articles { get; init; } = new Dictionary<Guid, Article>();
        public TimeSpan MaxAgeOfOldArticles { get; init; }
    }
}
