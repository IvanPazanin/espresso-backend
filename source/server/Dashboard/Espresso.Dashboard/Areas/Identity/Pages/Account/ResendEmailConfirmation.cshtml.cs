﻿// ResendEmailConfirmation.cshtml.cs
//
// © 2022 Espresso News. All rights reserved.

using System.Text.Encodings.Web;
using Espresso.Common.Services.Contacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Espresso.Dashboard.Areas.Identity.Pages.Account;

[AllowAnonymous]
#pragma warning disable SA1649 // File name should match first type name
public class ResendEmailConfirmationModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IEmailService _emailService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResendEmailConfirmationModel"/> class.
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="emailService"></param>
    public ResendEmailConfirmationModel(UserManager<IdentityUser> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    [BindProperty]
    public ResendEmailConformationInputModel Input { get; set; } = null!;

#pragma warning disable SA1201 // Elements should appear in the correct order
    public void OnGet()
#pragma warning restore SA1201 // Elements should appear in the correct order
    {
        // Method intentionally left empty.
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.FindByEmailAsync(Input.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
            return Page();
        }

        var userId = await _userManager.GetUserIdAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = Url.Page(
            "/Account/ConfirmEmail",
            pageHandler: null,
            values: new { userId, code },
            protocol: Request.Scheme)!;
        _ = await _emailService.SendMail(
            recipient: Input.Email,
            subject: "Confirm your email",
            content: $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
            htmlContent: $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
        return Page();
    }
}
