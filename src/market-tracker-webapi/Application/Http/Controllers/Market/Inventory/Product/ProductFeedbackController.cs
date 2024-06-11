using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product.Feedback;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Service.Operations.Market.Inventory.Product;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Market.Inventory.Product;

[ApiController]
[Produces(Uris.JsonMediaType, Problems.Problem.MediaType)]
public class ProductFeedbackController(IProductFeedbackService productFeedbackService) : ControllerBase
{
    [HttpGet(Uris.Products.ReviewsByProductId)]
    public async Task<ActionResult<PaginatedResult<ProductReviewOutputModel>>> GetReviewsByProductIdAsync(
        string productId, [FromQuery] PaginationInputs paginationInputs)
    {
        var reviews = await productFeedbackService.GetReviewsByProductIdAsync(
            productId, paginationInputs.Skip, paginationInputs.ItemsPerPage);
        
        return reviews.Select(review => review.ToOutputModel());
    }

    [HttpGet(Uris.Products.ProductPreferencesById)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ProductPreferencesOutputModel>> GetProductPreferencesAsync(string productId)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        return (await productFeedbackService.GetProductPreferencesAsync(
            authUser.User.Id.Value, productId)).ToOutputModel();
    }

    [HttpPatch(Uris.Products.ProductPreferencesById)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ProductPreferencesOutputModel>> AddUserFeedbackByProductIdAsync(
        string productId, [FromBody] ProductPreferencesInputModel preferencesInput)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        return (await productFeedbackService.UpsertProductPreferencesAsync(
            authUser.User.Id.Value,
            productId,
            preferencesInput.IsFavourite,
            preferencesInput.Review
        )).ToOutputModel();
    }

    [HttpGet(Uris.Products.StatsByProductId)]
    public async Task<ActionResult<ProductStats>> GetStatsByProductIdAsync(string productId)
    {
        return await productFeedbackService.GetProductStatsByIdAsync(productId);
    }
}