﻿using Espresso.Application.Infrastructure;
using Espresso.Common.Enums;
using Espresso.Domain.Enums.ApplicationDownloadEnums;

namespace Espresso.Application.CQRS.ApplicationDownloads.Queries.GetApplicationDownloadStatistics
{
    public class GetApplicationDownloadStatisticsQuery : Request<GetApplicationDownloadStatisticsQueryResponse>
    {
        public GetApplicationDownloadStatisticsQuery(
            string currentEspressoWebApiVersion,
            string targetedEspressoWebApiVersion,
            string consumerVersion,
            DeviceType deviceType
        ) : base(
            currentEspressoWebApiVersion: currentEspressoWebApiVersion,
            targetedEspressoWebApiVersion: targetedEspressoWebApiVersion,
            consumerVersion: consumerVersion,
            deviceType: deviceType,
            Event.GetApplicationDownloadStatisticsQuery
        )
        {
        }

        public override string ToString()
        {
            return "";
        }
    }
}
