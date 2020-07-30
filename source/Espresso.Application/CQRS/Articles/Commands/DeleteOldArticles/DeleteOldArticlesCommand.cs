﻿using System;
using Espresso.Application.Infrastructure;
using Espresso.Common.Enums;
using Espresso.Domain.Enums.ApplicationDownloadEnums;

namespace Espresso.Application.CQRS.Articles.Commands.DeleteOldArticles
{
    public class DeleteOldArticlesCommand : Request<DeleteOldArticlesCommandResponse>
    {
        #region Properties
        public TimeSpan MaxAgeOfOldArticles { get; }
        #endregion

        #region Constructors
        public DeleteOldArticlesCommand(
            TimeSpan maxAgeOfOldArticles,
            string currentEspressoWebApiVersion,
            string targetedEspressoWebApiVersion,
            string consumerVersion,
            DeviceType deviceType
        ) : base(
            currentEspressoWebApiVersion: currentEspressoWebApiVersion,
            targetedEspressoWebApiVersion: targetedEspressoWebApiVersion,
          consumerVersion: consumerVersion,
            deviceType: deviceType,
            Event.CalculateTrendingScoreCommand
        )
        {
            MaxAgeOfOldArticles = maxAgeOfOldArticles;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"{nameof(MaxAgeOfOldArticles)}:{MaxAgeOfOldArticles}";
        }
        #endregion
    }
}
