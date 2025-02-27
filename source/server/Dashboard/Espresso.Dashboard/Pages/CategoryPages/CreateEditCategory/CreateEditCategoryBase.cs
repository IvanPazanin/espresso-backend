﻿// CreateEditCategoryBase.cs
//
// © 2022 Espresso News. All rights reserved.

using Espresso.Application.DataTransferObjects.CategoryDataTransferObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Espresso.Dashboard.Pages.CategoryPages.CreateEditCategory;

/// <summary>
/// NewsPortalDetailsBase.
/// </summary>
public class CreateEditCategoryBase : ComponentBase
{
    private bool _success;

    [Parameter]
    [EditorRequired]
    public EventCallback<MouseEventArgs> OnSaveButtonClick { get; set; }

    [Parameter]
    [EditorRequired]
    public CategoryDto? Category { get; set; }

    [Parameter]
    public EventCallback<bool> SuccessChanged { get; set; }

#pragma warning disable BL0007 // Component parameter should be auto property
    [Parameter]
    public bool Success
    {
        get => _success;
        set
        {
            if (_success == value)
            {
                return;
            }

            _success = value;
            _ = SuccessChanged.InvokeAsync(value);
        }
    }
#pragma warning restore BL0007 // Component parameter should be auto property

#pragma warning disable CA1819 // Properties should not return arrays
    protected string[] Errors { get; set; } = [];
#pragma warning restore CA1819 // Properties should not return arrays

    protected MudForm? Form { get; set; }
}
