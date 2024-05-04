using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Product;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Operations.Product;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Market.Inventory.Product;

[ApiController]
public class ProductFeedbackController(IProductFeedbackService productFeedbackService) : ControllerBase
{
    [HttpGet(Uris.Products.ReviewsByProductId)]
    public async Task<ActionResult<PaginatedResult<ProductReviewOutputModel>>> GetReviewsByProductIdAsync(
        string productId,
        [FromQuery] PaginationInputs paginationInputs
    )
    {
        var res = await productFeedbackService.GetReviewsByProductIdAsync(
            productId, paginationInputs.Skip, paginationInputs.ItemsPerPage
        );
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ProductFetchingError.ProductByIdNotFound idNotFoundError
                        => new ProductProblem.ProductByIdNotFound(idNotFoundError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            }
        );
    }

    [HttpGet(Uris.Products.ProductPreferencesById)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ProductPreferences>> GetProductsPreferencesAsync(string productId)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var res = await productFeedbackService.GetProductsPreferencesAsync(
            authUser.User.Id, productId
        );
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ProductFetchingError.ProductByIdNotFound idNotFoundError
                        => new ProductProblem.ProductByIdNotFound(idNotFoundError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            }
        );
    }

    [HttpPatch(Uris.Products.ProductPreferencesById)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ProductPreferences>> AddUserFeedbackByProductIdAsync(
        string productId,
        [FromBody] ProductPreferencesInputModel preferencesInput
    )
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var res = await productFeedbackService.UpsertProductPreferencesAsync(
            authUser.User.Id,
            productId,
            preferencesInput.IsFavourite,
            preferencesInput.Review
        );

        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ProductFetchingError.ProductByIdNotFound idNotFoundError
                        => new ProductProblem.ProductByIdNotFound(idNotFoundError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            }
        );
    }
    
    [HttpGet(Uris.Products.StatsByProductId)]
    public async Task<ActionResult<ProductStats>> GetStatsByProductIdAsync(string productId)
    {
        var res = await productFeedbackService.GetProductStatsByIdAsync(productId);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ProductFetchingError.ProductByIdNotFound idNotFoundError
                        => new ProductProblem.ProductByIdNotFound(idNotFoundError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            }
        );
    }
}