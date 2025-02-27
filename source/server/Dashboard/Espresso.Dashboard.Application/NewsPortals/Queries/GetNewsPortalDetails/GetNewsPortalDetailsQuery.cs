﻿// GetNewsPortalDetailsQuery.cs
//
// © 2022 Espresso News. All rights reserved.

using Espresso.Domain.Entities;
using MediatR;

namespace Espresso.Dashboard.Application.NewsPortals.Queries.GetNewsPortalDetails;

/// <summary>
/// Get news portal details query.
/// </summary>
public class GetNewsPortalDetailsQuery : IRequest<GetNewsPortalDetailsQueryResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetNewsPortalDetailsQuery"/> class.
    /// </summary>
    /// <param name="newsPortalId"><see cref="NewsPortal"/> id.</param>
    public GetNewsPortalDetailsQuery(int newsPortalId)
    {
        NewsPortalId = newsPortalId;
    }

    /// <summary>
    /// Gets <see cref="NewsPortal"/> id.
    /// </summary>
    public int NewsPortalId { get; }
}
