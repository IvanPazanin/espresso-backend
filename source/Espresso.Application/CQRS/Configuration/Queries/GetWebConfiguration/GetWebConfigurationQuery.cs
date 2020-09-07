﻿using System;
using Espresso.Application.Infrastructure;
using Espresso.Common.Enums;
using Espresso.Domain.Enums.ApplicationDownloadEnums;

namespace Espresso.Application.CQRS.Configuration.Queries.GetWebConfiguration
{
    public class GetWebConfigurationQuery : Request<GetWebConfigurationQueryResponse>
    {
        public TimeSpan MaxAgeOfNewNewsPortal { get; }

        public GetWebConfigurationQuery(
            TimeSpan maxAgeOfNewNewsPortal,
            string currentEspressoWebApiVersion,
            string targetedEspressoWebApiVersion,
            string consumerVersion,
            DeviceType deviceType,
            AppEnvironment appEnvironment
        ) : base(
            currentEspressoWebApiVersion: currentEspressoWebApiVersion,
            targetedEspressoWebApiVersion: targetedEspressoWebApiVersion,
            consumerVersion: consumerVersion,
            deviceType: deviceType,
            appEnvironment: appEnvironment,
            Event.GetConfigurationQuery
        )
        {
            MaxAgeOfNewNewsPortal = maxAgeOfNewNewsPortal;
        }

        public override string ToString()
        {
            return $"{nameof(MaxAgeOfNewNewsPortal)}:{MaxAgeOfNewNewsPortal}";
        }
    }
}
