﻿using System;
using Espresso.WebApi.Application.Configuration.Queries.GetConfiguration;
using Espresso.WebApi.Configuration;
using Espresso.WebApi.GraphQl.ApplicationTypes.ConfigurationTypes.GetConfigurationTypes;
using Espresso.WebApi.GraphQl.Infrastructure;
using GraphQL.Types;
using MediatR;

namespace Espresso.WebApi.GraphQl.ApplicationQueries.ConfigurationQueries
{
    /// <summary>
    /// 
    /// </summary>
    public class GetConfigurationGraphQlQuery : ObjectGraphType, IGraphQlQuery
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="webApiConfiguration"></param>
        public GetConfigurationGraphQlQuery(
            IMediator mediator,
            IWebApiConfiguration webApiConfiguration
        )
        {
            FieldAsync<GetConfigurationQueryResponseType>(
                name: "configuration",
                arguments: null,
                resolve: async resolveContext =>
                {
                    var userContext = resolveContext.UserContext as GraphQlUserContext ??
                        throw new Exception("Invalid GraphQL User Context");

                    return await mediator.Send(
                        request: new GetConfigurationQuery
                        {
                            MaxAgeOfNewNewsPortal = webApiConfiguration.DateTimeConfiguration.MaxAgeOfNewNewsPortal,
                            CurrentApiVersion = webApiConfiguration.AppConfiguration.Version,
                            TargetedApiVersion = userContext.TargetedApiVersion,
                            ConsumerVersion = userContext.ConsumerVersion,
                            DeviceType = userContext.DeviceType,
                            AppEnvironment = webApiConfiguration.AppConfiguration.AppEnvironment
                        },
                        cancellationToken: resolveContext.CancellationToken
                    );
                },
                deprecationReason: null
            );
        }
    }
}
