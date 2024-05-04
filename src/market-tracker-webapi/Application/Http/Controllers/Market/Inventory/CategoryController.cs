using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Category;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Service.Errors.Category;
using market_tracker_webapi.Application.Service.Operations.Category;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Market.Inventory;

[ApiController]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet(Uris.Categories.Base)]
    public async Task<ActionResult<CollectionOutputModel<Category>>> GetCategoriesAsync()
    {
        var res = await categoryService.GetCategoriesAsync();
        return ResultHandler.Handle(
            res,
            _ => new ServerProblem.InternalServerError().ToActionResult()
        );
    }

    [HttpGet(Uris.Categories.CategoryById)]
    public async Task<ActionResult<Category>> GetCategoryAsync(int categoryId)
    {
        var res = await categoryService.GetCategoryAsync(categoryId);
        return ResultHandler.Handle(
            res,
            _ => new ServerProblem.InternalServerError().ToActionResult()
        );
    }

    [HttpPost(Uris.Categories.Base)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<IntIdOutputModel>> AddCategoryAsync(
        [FromBody] CategoryInputModel categoryInput
    )
    {
        var res = await categoryService.AddCategoryAsync(categoryInput.Name);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    CategoryFetchingError.CategoryByIdNotFound idNotFoundError
                        => new CategoryProblem.CategoryByIdNotFound(
                            idNotFoundError
                        ).ToActionResult(),

                    CategoryCreationError.CategoryNameAlreadyExists _
                        => new CategoryProblem.CategoryNameAlreadyExists().ToActionResult(),

                    CategoryCreationError.InvalidParentCategory invalidParentCategoryIdError
                        => new CategoryProblem.InvalidParentCategory(
                            invalidParentCategoryIdError
                        ).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            },
            outputModel =>
                Created(Uris.Categories.BuildCategoryByIdUri(outputModel.Id), outputModel)
        );
    }

    [HttpPut(Uris.Categories.CategoryById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<Category>> UpdateCategoryAsync(
        int categoryId,
        [FromBody] CategoryInputModel categoryInput
    )
    {
        var res = await categoryService.UpdateCategoryAsync(categoryId, categoryInput.Name);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    CategoryFetchingError.CategoryByIdNotFound idNotFoundError
                        => new CategoryProblem.CategoryByIdNotFound(
                            idNotFoundError
                        ).ToActionResult(),

                    CategoryCreationError.CategoryNameAlreadyExists _
                        => new CategoryProblem.CategoryNameAlreadyExists().ToActionResult(),

                    CategoryCreationError.InvalidParentCategory invalidParentCategoryIdError
                        => new CategoryProblem.InvalidParentCategory(
                            invalidParentCategoryIdError
                        ).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            }
        );
    }

    [HttpDelete(Uris.Categories.CategoryById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<IntIdOutputModel>> RemoveCategoryAsync(int categoryId)
    {
        var res = await categoryService.RemoveCategoryAsync(categoryId);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    CategoryFetchingError.CategoryByIdNotFound idNotFoundError
                        => new CategoryProblem.CategoryByIdNotFound(
                            idNotFoundError
                        ).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            },
            _ => NoContent()
        );
    }
}