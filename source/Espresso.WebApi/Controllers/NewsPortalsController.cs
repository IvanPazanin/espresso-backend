﻿using System.Threading;
using System.Threading.Tasks;
using Espresso.Application.CQRS.NewsPortals.Queries.GetNewsPortals;
using Espresso.Common.Constants;
using Espresso.WebApi.Configuration;
using Espresso.WebApi.HeaderParameters;
using Espresso.WebApi.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Espresso.Application.CQRS.NewsPortals.Commands.NewSourcesRequest;
using Espresso.WebApi.RequestObject;

namespace Espresso.WebApi.Controllers
{
    /// <summary>
    /// News Portals Controller
    /// </summary>
    public class NewsPortalsController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="webApiConfiguration"></param>
        public NewsPortalsController(
            IMediator mediator,
            IWebApiConfiguration webApiConfiguration
        ) : base(mediator, webApiConfiguration)
        {
        }

        /// <summary>
        /// Get all Espresso news portals
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     Get /api/newsportals
        /// </remarks>
        /// <param name="cancellationToken"></param>
        /// <param name="basicInformationsHeaderParameters"></param>
        /// <returns>Response object containing Espresso newsportals</returns>
        /// <response code="200">Response object containing Espresso newsportals</response>
        /// <response code="401">If API Key is invalid or missing</response>
        /// <response code="500">If unhandled exception occurred</response>
        [Produces(MimeTypeConstants.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetNewsPortalsQueryResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Route("api/newsportals")]
        public async Task<IActionResult> GetNewsPortals(
            [FromHeader] BasicInformationsHeaderParameters basicInformationsHeaderParameters,
            CancellationToken cancellationToken
        )
        {
            var getNewsPortalsQueryResponse = await Mediator.Send(
                request: new GetNewsPortalsQuery(
                    currentEspressoWebApiVersion: WebApiConfiguration.Version,
                    espressoWebApiVersion: basicInformationsHeaderParameters.EspressoWebApiVersion,
                    version: basicInformationsHeaderParameters.Version,
                    deviceType: basicInformationsHeaderParameters.DeviceType
                ),
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);

            return Ok(getNewsPortalsQueryResponse);
        }


        /// <summary>
        /// Request new NewsPortal
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST /api/newsportals
        /// </remarks>
        /// <param name="cancellationToken"></param>
        /// <param name="basicInformationsHeaderParameters"></param>
        /// <param name="requestNewsPortalRequestObject"></param>
        /// <returns>Response object containing Espresso newsportals</returns>
        /// <response code="200">Response object containing Espresso newsportals</response>
        /// <response code="401">If API Key is invalid or missing</response>
        /// <response code="500">If unhandled exception occurred</response>
        [Produces(MimeTypeConstants.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetNewsPortalsQueryResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("api/newsportals")]
        public async Task<IActionResult> RequestNewsPortal(
            [FromHeader] BasicInformationsHeaderParameters basicInformationsHeaderParameters,
            [FromBody] RequestNewsPortalRequestObject requestNewsPortalRequestObject,
            CancellationToken cancellationToken
        )
        {
            var getNewsPortalsQueryResponse = await Mediator.Send(
                request: new NewsSourcesRequestCommand(
                    currentEspressoWebApiVersion: WebApiConfiguration.Version,
                    espressoWebApiVersion: basicInformationsHeaderParameters.EspressoWebApiVersion,
                    version: basicInformationsHeaderParameters.Version,
                    deviceType: basicInformationsHeaderParameters.DeviceType,
                    newsPortalName: requestNewsPortalRequestObject.NewsPortalName ?? "",
                    email: requestNewsPortalRequestObject.Email ?? "",
                    url: requestNewsPortalRequestObject.Url
                ),
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);

            return Ok(getNewsPortalsQueryResponse);
        }
    }
}
