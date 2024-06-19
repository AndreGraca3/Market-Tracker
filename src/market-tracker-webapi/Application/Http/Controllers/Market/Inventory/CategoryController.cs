using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Category;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Service.Operations.Market.Inventory.Category;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Market.Inventory;

[ApiController]
[Produces(Uris.JsonMediaType, Problems.Problem.MediaType)]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet(Uris.Categories.Base)]
    public async Task<ActionResult<CollectionOutputModel<CategoryOutputModel>>> GetCategoriesAsync()
    {
        var categories = await categoryService.GetCategoriesAsync();
        return categories.Select(c => c.ToOutputModel()).ToCollectionOutputModel();
    }

    [HttpGet(Uris.Categories.CategoryById)]
    public async Task<ActionResult<CategoryOutputModel>> GetCategoryAsync(int categoryId)
    {
        return (await categoryService.GetCategoryAsync(categoryId)).ToOutputModel();
    }

    [HttpPost(Uris.Categories.Base)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<CategoryId>> AddCategoryAsync(
        [FromBody] CategoryInputModel categoryInput
    )
    {
        var categoryId = await categoryService.AddCategoryAsync(categoryInput.Name);
        return Created(Uris.Categories.BuildCategoryByIdUri(categoryId.Value), categoryId);
    }

    [HttpPut(Uris.Categories.CategoryById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<CategoryOutputModel>> UpdateCategoryAsync(
        int categoryId,
        [FromBody] CategoryInputModel categoryInput
    )
    {
        return (await categoryService.UpdateCategoryAsync(categoryId, categoryInput.Name)).ToOutputModel();
    }

    [HttpDelete(Uris.Categories.CategoryById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult> RemoveCategoryAsync(int categoryId)
    {
        await categoryService.RemoveCategoryAsync(categoryId);
        return NoContent();
    }
}