using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Service;
using market_tracker_webapi.Application.Service.Errors.Category;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers;

public class CategoryController(CategoryService categoryService) : ControllerBase
{
    [HttpGet(Uris.Categories.Base)]
    public async Task<ActionResult<CategoryOutputModel>> GetCategoriesAsync()
    {
        var categories = await categoryService.GetCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet(Uris.Categories.CategoryById)]
    public async Task<ActionResult<CategoryOutputModel>> GetCategoryAsync(int id)
    {
        var res = await categoryService.GetCategoryAsync(id);
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
                };
            }
        );
    }

    [HttpPost(Uris.Categories.Base)]
    public async Task<ActionResult<IdOutputModel>> AddCategoryAsync(
        [FromBody] CategoryCreationInputModel categoryInput
    )
    {
        var res = await categoryService.AddCategoryAsync(
            categoryInput.Name,
            categoryInput.parentId
        );
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    CategoryCreationError.CategoryNameAlreadyExists _
                        => new CategoryProblem.CategoryNameAlreadyExists().ToActionResult(),

                    CategoryCreationError.InvalidName invalidNameError
                        => new CategoryProblem.InvalidName(invalidNameError).ToActionResult()
                };
            }
        );
    }

    [HttpDelete(Uris.Categories.CategoryById)]
    public async Task<ActionResult<IdOutputModel>> RemoveCategoryAsync(int id)
    {
        var res = await categoryService.RemoveCategoryAsync(id);
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
                };
            }
        );
    }
}
