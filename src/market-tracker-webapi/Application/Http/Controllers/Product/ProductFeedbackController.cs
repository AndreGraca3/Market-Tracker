using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Product;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Operations.Product;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Product;

[ApiController]
public class ProductFeedbackController(IProductFeedbackService productFeedbackService)
    : ControllerBase
{
    [HttpGet(Uris.Products.ReviewsByProductId)]
    public async Task<ActionResult<CollectionOutputModel>> GetReviewsByProductIdAsync(int productId)
    {
        var res = await productFeedbackService.GetReviewsByProductIdAsync(productId);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ProductFetchingError.ProductByIdNotFound idNotFoundError
                        => new ProductProblem.ProductByIdNotFound(idNotFoundError).ToActionResult()
                };
            }
        );
    }

    [HttpPut(Uris.Products.ReviewsByProductId)]
    public async Task<ActionResult<IdOutputModel>> AddReviewAsync(
        int productId,
        [FromBody] ProductReviewInputModel productReviewInput
    )
    {
        var clientId = Guid.NewGuid(); // TODO: Implement authorization
        var res = await productFeedbackService.UpsertReviewAsync(
            clientId,
            productId,
            productReviewInput.Rating,
            productReviewInput.Comment
        );

        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ProductFetchingError.ProductByIdNotFound idNotFoundError
                        => new ProductProblem.ProductByIdNotFound(idNotFoundError).ToActionResult(),
                };
            },
            outputModel =>
                Created(Uris.Products.BuildReviewsByProductIdUri(outputModel.Id), outputModel)
        );
    }

    [HttpGet(Uris.Products.ProductPreferencesById)]
    public async Task<ActionResult<ProductPreferences>> GetUserFeedbackByProductId(int productId)
    {
        var clientId = Guid.NewGuid(); // TODO: Implement authorization
        var res = await productFeedbackService.GetUserFeedbackByProductId(clientId, productId);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ProductFetchingError.ProductByIdNotFound idNotFoundError
                        => new ProductProblem.ProductByIdNotFound(idNotFoundError).ToActionResult()
                };
            }
        );
    }

    [HttpGet(Uris.Products.StatsByProductId)]
    public async Task<ActionResult<ProductStats>> GetStatsByProductIdAsync(int productId)
    {
        var res = await productFeedbackService.GetProductStatsByIdAsync(productId);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ProductFetchingError.ProductByIdNotFound idNotFoundError
                        => new ProductProblem.ProductByIdNotFound(idNotFoundError).ToActionResult()
                };
            }
        );
    }
}
