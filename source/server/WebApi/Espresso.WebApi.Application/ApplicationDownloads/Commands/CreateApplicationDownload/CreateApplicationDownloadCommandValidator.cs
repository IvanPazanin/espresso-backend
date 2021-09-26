﻿// CreateApplicationDownloadCommandValidator.cs
//
// © 2021 Espresso News. All rights reserved.

using Espresso.Common.Enums;
using Espresso.Domain.Entities;
using FluentValidation;
using System;

namespace Espresso.WebApi.Application.ApplicationDownloads.Commands.CreateApplicationDownload
{
    public class CreateApplicationDownloadCommandValidator : AbstractValidator<CreateApplicationDownloadCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateApplicationDownloadCommandValidator"/> class.
        /// </summary>
        public CreateApplicationDownloadCommandValidator()
        {
            RuleFor(createApplicationDownloadCommand => createApplicationDownloadCommand.ConsumerVersion).NotEmpty().MaximumLength(ApplicationDownload.MobileAppVersionMaxLenght);

            RuleFor(createApplicationDownloadCommand => createApplicationDownloadCommand.DeviceType).Must(mobileDeviceType => Enum.IsDefined(typeof(DeviceType), mobileDeviceType) && mobileDeviceType != DeviceType.Undefined);
        }
    }
}
