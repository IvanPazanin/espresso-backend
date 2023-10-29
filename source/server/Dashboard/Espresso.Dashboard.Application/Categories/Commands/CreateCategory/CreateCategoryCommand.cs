// CreateCategoryCommand.cs
//
// © 2022 Espresso News. All rights reserved.

using Espresso.Application.DataTransferObjects.CategoryDataTransferObjects;
using MediatR;

namespace Espresso.Dashboard.Application.Categories.Commands.UpdateCategory;

public class CreateCategoryCommand : IRequest
{
    public CreateCategoryCommand(CategoryDto category)
    {
        Category = category;
    }

    public CategoryDto Category { get; }
}
