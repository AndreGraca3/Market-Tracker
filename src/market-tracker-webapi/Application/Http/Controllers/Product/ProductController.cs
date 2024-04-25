using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Price;
using market_tracker_webapi.Application.Http.Models.Product;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Product;
using market_tracker_webapi.Application.Service.Errors.Category;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Operations.Product;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Product;

[ApiController]
public class ProductController(IProductService productService, IProductPriceService productPriceService)
    : ControllerBase
{
    [HttpGet(Uris.Products.Base)]
    public async Task<ActionResult<PaginatedProductOffers>> GetProductsAsync(
        [FromQuery] ProductsFiltersInputModel filters,
        [FromQuery] PaginationInputs paginationInputs,
        [FromQuery] SortByType? sortBy
    )
    {
        var res =
            await productService.GetProductOffersAsync(
                paginationInputs.Skip,
                paginationInputs.ItemsPerPage,
                sortBy,
                filters.SearchName,
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
    public async Task<ActionResult<StringIdOutputModel>> AddProductAsync(
        [FromBody] ProductCreationInputModel productInput
    )
    {
        var res = await productService.AddProductAsync(
            productInput.Id,
            productInput.Name,
            productInput.ImageUrl,
            productInput.Quantity,
            productInput.Unit,
            productInput.BrandName,
            productInput.CategoryId!.Value
        );

        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ProductCreationError.ProductAlreadyExists alreadyExists
                        => new ProductProblem.ProductAlreadyExists(alreadyExists).ToActionResult(),

                    CategoryFetchingError.CategoryByIdNotFound categoryNotFound
                        => new CategoryProblem.CategoryByIdNotFound(
                            categoryNotFound
                        ).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            },
            outputModel => Created(Uris.Products.BuildProductByIdUri(outputModel.Id), outputModel)
        );
    }

    [HttpPatch(Uris.Products.ProductById)]
    public async Task<ActionResult<ProductInfoOutputModel>> UpdateProductAsync(
        string productId,
        [FromBody] ProductUpdateInputModel productInput
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