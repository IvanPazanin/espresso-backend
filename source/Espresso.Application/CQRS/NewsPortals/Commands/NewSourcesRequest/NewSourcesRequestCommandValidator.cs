﻿using FluentValidation;

namespace Espresso.Application.CQRS.NewsPortals.Commands.NewSourcesRequest
{
    public class NewSourcesRequestCommandValidator : AbstractValidator<NewsSourcesRequestCommand>
    {
        public NewSourcesRequestCommandValidator()
        {
            _ = RuleFor(request => request.NewsPortalName).NotEmpty();
            _ = RuleFor(request => request.Email).NotEmpty().EmailAddress();
        }
    }
}
