﻿using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Espresso.WebApi.Application.ApplicationDownloads.Commands.CreateApplicationDownload;
using Espresso.WebApi.Application.ApplicationDownloads.Queries.GetApplicationDownloadStatistics;
using Espresso.Common.Constants;
using Espresso.Domain.Enums.ApplicationDownloadEnums;
using Espresso.WebApi.Authentication;
using Espresso.WebApi.Configuration;
using Espresso.WebApi.Infrastructure;
using Espresso.WebApi.Parameters.HeaderParameters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Espresso.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationDownloadsController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="webApiConfiguration"></param>
        public ApplicationDownloadsController(
            IMediator mediator,
            IWebApiConfiguration webApiConfiguration
        ) : base(mediator, webApiConfiguration)
        {
        }


        /// <summary>
        /// Creates New Application Download
        /// </summary>
        /// <param name="basicInformationsHeaderParameters"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code="200">Response object containing articles from provided category</response>
        /// <response code="401">If API Key is invalid or missing</response>
        /// <response code="500">If unhandled exception occurred</response>
        [Produces(MimeTypeConstants.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiVersion("1.4")]
        [HttpPost]
        [Authorize(Roles = ApiKey.DevMobileAppRole + "," + ApiKey.MobileAppRole + "," + ApiKey.WebAppRole)]
        [Route("api/application-downloads")]
        public async Task<IActionResult> CreateApplicationDownload(
            [FromHeader] BasicInformationsHeaderParameters basicInformationsHeaderParameters,
            CancellationToken cancellationToken
        )
        {
            var command = new CreateApplicationDownloadCommand(
                currentEspressoWebApiVersion: WebApiConfiguration.AppVersionConfiguration.Version,
                targetedEspressoWebApiVersion: basicInformationsHeaderParameters.EspressoWebApiVersion,
                consumerVersion: basicInformationsHeaderParameters.Version,
                deviceType: basicInformationsHeaderParameters.DeviceType,
                appEnvironment: WebApiConfiguration.AppConfiguration.AppEnvironment
            );

            await Mediator.Send(
                request: command,
                cancellationToken: cancellationToken
            );

            return Ok();
        }

        /// <summary>
        /// Creates New Application Download
        /// </summary>
        /// <param name="basicInformationsHeaderParameters"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code="200">Response object containing articles from provided category</response>
        /// <response code="401">If API Key is invalid or missing</response>
        /// <response code="500">If unhandled exception occurred</response>
        [Produces(MimeTypeConstants.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiVersion("1.3")]
        [HttpPost]
        [Authorize(Roles = ApiKey.DevMobileAppRole + "," + ApiKey.MobileAppRole + "," + ApiKey.WebAppRole)]
        [Route("api/applicationDownloads/create")]
        public async Task<IActionResult> CreateApplicationDownload_1_3(
            [FromHeader] BasicInformationsHeaderParameters basicInformationsHeaderParameters,
            CancellationToken cancellationToken
        )
        {
            var command = new CreateApplicationDownloadCommand(
                currentEspressoWebApiVersion: WebApiConfiguration.AppVersionConfiguration.Version,
                targetedEspressoWebApiVersion: basicInformationsHeaderParameters.EspressoWebApiVersion,
                consumerVersion: basicInformationsHeaderParameters.Version,
                deviceType: basicInformationsHeaderParameters.DeviceType,
                appEnvironment: WebApiConfiguration.AppConfiguration.AppEnvironment
            );

            await Mediator.Send(
                request: command,
                cancellationToken: cancellationToken
            );

            return Ok();
        }

        /// <summary>
        /// Creates New Application Download
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="mobileAppVersion">Mobile App Version</param>
        /// <param name="mobileDeviceType">Mobile Device Type (1 = Android, 2 = Ios)</param>
        /// <returns></returns>
        /// <response code="200">Response object containing articles from provided category</response>
        /// <response code="400">If <paramref name="mobileAppVersion"/> is empty or longer than 20 characters or deviceType is not 1 or 2</response>
        /// <response code="401">If API Key is invalid or missing</response>
        /// <response code="500">If unhandled exception occurred</response>
        [Produces(MimeTypeConstants.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiVersion("1.2")]
        [HttpPost]
        [Authorize(Roles = ApiKey.DevMobileAppRole + "," + ApiKey.MobileAppRole + "," + ApiKey.WebAppRole)]
        [Route("api/applicationDownloads/create")]
        public async Task<IActionResult> CreateApplicationDownload_1_2(
            [Required] string mobileAppVersion,
            [Required] DeviceType mobileDeviceType,
            CancellationToken cancellationToken
        )
        {
            var command = new CreateApplicationDownloadCommand(
                currentEspressoWebApiVersion: WebApiConfiguration.AppVersionConfiguration.Version,
                targetedEspressoWebApiVersion: WebApiConfiguration.AppVersionConfiguration.EspressoWebApiVersion_1_2.ToString(),
                consumerVersion: mobileAppVersion,
                deviceType: mobileDeviceType,
                appEnvironment: WebApiConfiguration.AppConfiguration.AppEnvironment
            );
            await Mediator.Send(
                request: command,
                cancellationToken: cancellationToken
            );

            return Ok();
        }

        /// <summary>
        /// Gets App Downloads Statistics
        /// </summary>
        /// <param name="basicInformationsHeaderParameters"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Response object containing articles from provided category</returns>
        /// <response code="200">Response object containing articles from popular news portals</response>
        /// <response code="401">If API Key is invalid or missing</response>
        /// <response code="500">If unhandled exception occurred</response>
        [Produces(MimeTypeConstants.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetApplicationDownloadStatisticsQueryResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiVersion("1.4")]
        [HttpGet]
        [Authorize(Roles = ApiKey.DevMobileAppRole + "," + ApiKey.MobileAppRole + "," + ApiKey.WebAppRole)]
        [Route("api/application-downloads")]
        public async Task<IActionResult> GetApplicationDownloadsStatistics(
            [FromHeader] BasicInformationsHeaderParameters basicInformationsHeaderParameters,
            CancellationToken cancellationToken
        )
        {
            var response = await Mediator.Send(
                request: new GetApplicationDownloadStatisticsQuery(
                    currentEspressoWebApiVersion: WebApiConfiguration.AppVersionConfiguration.Version,
                    targetedEspressoWebApiVersion: basicInformationsHeaderParameters.EspressoWebApiVersion,
                    consumerVersion: basicInformationsHeaderParameters.Version,
                    deviceType: basicInformationsHeaderParameters.DeviceType,
                    appEnvironment: WebApiConfiguration.AppConfiguration.AppEnvironment
                ),
                cancellationToken: cancellationToken
            );

            return Ok(response);
        }


        /// <summary>
        /// Gets App Downloads Statistics
        /// </summary>
        /// <param name="basicInformationsHeaderParameters"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Response object containing articles from provided category</returns>
        /// <response code="200">Response object containing articles from popular news portals</response>
        /// <response code="401">If API Key is invalid or missing</response>
        /// <response code="500">If unhandled exception occurred</response>
        [Produces(MimeTypeConstants.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetApplicationDownloadStatisticsQueryResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiVersion("1.3")]
        [ApiVersion("1.2")]
        [HttpGet]
        [Authorize(Roles = ApiKey.DevMobileAppRole + "," + ApiKey.MobileAppRole + "," + ApiKey.WebAppRole)]
        [Route("api/applicationDownloads/statistics")]
        public async Task<IActionResult> GetApplicationDownloadsStatistics_1_3(
            [FromHeader] BasicInformationsHeaderParameters basicInformationsHeaderParameters,
            CancellationToken cancellationToken
        )
        {
            var response = await Mediator.Send(
                request: new GetApplicationDownloadStatisticsQuery(
                    currentEspressoWebApiVersion: WebApiConfiguration.AppVersionConfiguration.Version,
                    targetedEspressoWebApiVersion: basicInformationsHeaderParameters.EspressoWebApiVersion,
                    consumerVersion: basicInformationsHeaderParameters.Version,
                    deviceType: basicInformationsHeaderParameters.DeviceType,
                    appEnvironment: WebApiConfiguration.AppConfiguration.AppEnvironment
                ),
                cancellationToken: cancellationToken
            );

            return Ok(response);
        }
    }
}
