using market_tracker_webapi.Application.Domain.Filters.Product;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Http.Models.Price;
using market_tracker_webapi.Application.Http.Models.Product;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Service.Errors.Category;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Service.Operations.Market.Inventory.Product;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Market.Inventory.Product;

[ApiController]
public class ProductController(IProductService productService, IProductPriceService productPriceService)
    : ControllerBase
{
    [HttpGet(Uris.Products.Base)]
    public async Task<ActionResult<PaginatedProductOffers>> GetProductsAsync(
        [FromQuery] ProductsFiltersInputModel filters,
        [FromQuery] PaginationInputs paginationInputs,
        [FromQuery] ProductsSortOption? sortBy
    )
    {
        var res =
            await productService.GetBestAvailableProductsOffersAsync(
                paginationInputs.Skip,
                paginationInputs.ItemsPerPage,
                sortBy,
                filters.Name,
                filters.CategoryIds,
                filters.BrandIds,
                filters.CompanyIds,
                filters.MinPrice,
                filters.MaxPrice,
                filters.MinRating,
                filters.MaxRating
            );
        return ResultHandler.Handle(
            res,
            _ => new ServerProblem.InternalServerError().ToActionResult()
        );
    }

    [HttpGet(Uris.Products.ProductById)]
    public async Task<ActionResult<ProductInfo>> GetProductByIdAsync(string productId)
    {
        var res = await productService.GetProductByIdAsync(productId);

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

    [HttpGet(Uris.Products.PricesByProductId)]
    public async Task<ActionResult<CollectionOutputModel<CompanyPricesOutputModel>>> GetProductPricesAsync(
        string productId)
    {
        var res = await productPriceService.GetProductPricesAsync(productId);

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

    [HttpPost(Uris.Products.Base)]
    [Authorized([Role.Operator])]
    public async Task<ActionResult<ProductCreationOutputModel>> AddProductAsync(
        [FromBody] ProductCreationInputModel productInput
    )
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var res = await productService.AddProductAsync(
            authUser.User.Id,
            productInput.Id,
            productInput.Name,
            productInput.ImageUrl,
            productInput.Quantity,
            productInput.Unit,
            productInput.BrandName,
            productInput.CategoryId!.Value,
            productInput.Price!.Value,
            productInput.PromotionPercentage
        );

        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    CategoryFetchingError.CategoryByIdNotFound categoryNotFound
                        => new CategoryProblem.CategoryByIdNotFound(
                            categoryNotFound
                        ).ToActionResult(),
                    StoreFetchingError.StoreByOperatorIdNotFound storeNotFound
                        => new StoreProblem.StoreByOperatorIdNotFound(storeNotFound).ToActionResult(),

                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            },
            outputModel =>
                outputModel.IsNew
                    ? Created(Uris.Products.BuildProductByIdUri(outputModel.Id), outputModel)
                    : Ok(outputModel));
    }

    [HttpPut(Uris.Products.AvailabilityByProductId)]
    [Authorized([Role.Operator])]
    public async Task<ActionResult<StringIdOutputModel>> SetProductAvailabilityAsync(
        string productId, [FromBody] ProductAvailabilityInputModel availabilityInput
    )
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var res = await productService.SetProductAvailabilityAsync(
            authUser.User.Id,
            productId,
            availabilityInput.IsAvailable
        );

        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ProductFetchingError.ProductByIdNotFound idNotFoundError
                        => new ProductProblem.ProductByIdNotFound(idNotFoundError).ToActionResult(),
                    StoreFetchingError.StoreByOperatorIdNotFound storeNotFound
                        => new StoreProblem.StoreByOperatorIdNotFound(storeNotFound).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            },
            _ => NoContent()
        );
    }

    [HttpPatch(Uris.Products.ProductById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<ProductInfoOutputModel>> UpdateProductAsync(
        string productId, [FromBody] ProductUpdateInputModel productInput
    )
    {
        var res = await productService.UpdateProductAsync(
            productId,
            productInput.Name,
            productInput.ImageUrl,
            productInput.Quantity,
            productInput.Unit,
            productInput.BrandName,
            productInput.CategoryId
        );

        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ProductFetchingError.ProductByIdNotFound idNotFoundError
                        => new ProductProblem.ProductByIdNotFound(idNotFoundError).ToActionResult(),

                    CategoryFetchingError.CategoryByIdNotFound categoryNotFound
                        => new CategoryProblem.CategoryByIdNotFound(
                            categoryNotFound
                        ).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            }
        );
    }

    [HttpDelete(Uris.Products.ProductById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<StringIdOutputModel>> RemoveProductAsync(string productId)
    {
        var res = await productService.RemoveProductAsync(productId);

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
            },
            _ => NoContent()
        );
    }
}