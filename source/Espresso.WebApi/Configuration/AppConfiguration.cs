﻿using System;
using System.Diagnostics;
using Espresso.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Espresso.WebApi.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class AppConfiguration
    {
        #region Fields
        private readonly IConfigurationSection _configuration;
        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public AppEnvironment AppEnvironment => _configuration.GetValue<AppEnvironment>("Environment");


        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public int NewNewsPortalsPosition => _configuration.GetValue<int>("NewNewsPortalsPosition");

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Version => _configuration.GetValue<string>("Version");

        /// <summary>
        /// 
        /// </summary>
        public ApiVersion EspressoWebApiVersion_1_2 => new ApiVersion(1, 2);

        /// <summary>
        /// 
        /// </summary>
        public ApiVersion EspressoWebApiVersion_1_3 => new ApiVersion(1, 3);

        /// <summary>
        /// 
        /// </summary>
        public ApiVersion EspressoWebApiCurrentVersion => new ApiVersion(
            majorVersion: _configuration.GetValue<int>("MajorVersion"),
            minorVersion: _configuration.GetValue<int>("MinorVersion")
        );

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TimeSpan Uptime => DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int FeaturedArticlesTake => _configuration.GetValue<int>("FeaturedArticlesTake");

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string SlackWebHook => _configuration.GetValue<string>("SlackWebHook");
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public AppConfiguration(IConfigurationSection configuration)
        {
            _configuration = configuration;
        }
        #endregion
    }
}
