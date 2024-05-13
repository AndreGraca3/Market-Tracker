using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Service.Operations.Market.Inventory.Product;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Market.Inventory.Product;

[ApiController]
public class ProductFeedbackController(IProductFeedbackService productFeedbackService) : ControllerBase
{
    [HttpGet(Uris.Products.ReviewsByProductId)]
    public async Task<ActionResult<PaginatedResult<ProductReview>>> GetReviewsByProductIdAsync(
        string productId,
        [FromQuery] PaginationInputs paginationInputs
    )
    {
        return await productFeedbackService.GetReviewsByProductIdAsync(
            productId, paginationInputs.Skip, paginationInputs.ItemsPerPage);
    }

    [HttpGet(Uris.Products.ProductPreferencesById)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ProductPreferences>> GetProductsPreferencesAsync(string productId)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        return await productFeedbackService.GetProductsPreferencesAsync(
            authUser.User.Id.Value, productId);
    }

    [HttpPatch(Uris.Products.ProductPreferencesById)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ProductPreferences>> AddUserFeedbackByProductIdAsync(
        string productId,
        [FromBody] ProductPreferencesInputModel preferencesInput
    )
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        return await productFeedbackService.UpsertProductPreferencesAsync(
            authUser.User.Id.Value,
            productId,
            preferencesInput.IsFavourite,
            preferencesInput.Review
        );
    }

    [HttpGet(Uris.Products.StatsByProductId)]
    public async Task<ActionResult<ProductStats>> GetStatsByProductIdAsync(string productId)
    {
        return await productFeedbackService.GetProductStatsByIdAsync(productId);
    }
}