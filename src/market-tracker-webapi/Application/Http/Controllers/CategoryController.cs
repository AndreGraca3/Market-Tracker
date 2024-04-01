using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Service.Errors.Category;
using market_tracker_webapi.Application.Service.Operations.Category;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers;

[ApiController]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet(Uris.Categories.Base)]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesAsync()
    {
        var categories = await categoryService.GetCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet(Uris.Categories.CategoryById)]
    public async Task<ActionResult<Category>> GetCategoryAsync(int id)
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
                        ).ToActionResult()
                };
            },
            outputModel =>
                Created(Uris.Categories.BuildCategoryByIdUri(outputModel.Id), outputModel)
        );
    }

    [HttpPut(Uris.Categories.CategoryById)]
    public async Task<ActionResult<IdOutputModel>> UpdateCategoryAsync(
        int id,
        [FromBody] CategoryInputModel categoryInput
    )
    {
        var res = await categoryService.UpdateCategoryAsync(id, categoryInput.Name);
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
                        ).ToActionResult()
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
            },
            _ => NoContent()
        );
    }
}
