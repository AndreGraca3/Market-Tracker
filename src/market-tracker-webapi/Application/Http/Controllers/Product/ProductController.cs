using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Product;
using market_tracker_webapi.Application.Http.Problem;
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
    public async Task<ActionResult<CollectionOutputModel>> GetProductsAsync(
        string? searchName,
        int? minRating,
        int? maxRating
    )
    {
        var res = await productService.GetProductsAsync(searchName, minRating, maxRating);
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
                };
            }
        );
    }

    [HttpGet(Uris.Products.PricesByProductId)]
    public async Task<ActionResult<CollectionOutputModel>> GetProductPricesAsync(string productId)
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
                };
            },
            outputModel => Created(Uris.Products.BuildProductByIdUri(outputModel.Id), outputModel)
        );
    }

    [HttpPut(Uris.Products.ProductById)]
    public async Task<ActionResult<ProductInfoOutputModel>> UpdateProductAsync(
        string productId,
        [FromBody] ProductUpdateInputModel productInput
    )
    {
        var res = await productService.UpdateProductAsync(
            productId,
            productInput.Name,
            productInput.ImageUrl,
            productInput.Quantity!.Value,
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
                    ProductFetchingError.ProductByIdNotFound idNotFoundError
                        => new ProductProblem.ProductByIdNotFound(idNotFoundError).ToActionResult(),

                    CategoryFetchingError.CategoryByIdNotFound categoryNotFound
                        => new CategoryProblem.CategoryByIdNotFound(
                            categoryNotFound
                        ).ToActionResult(),
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
                };
            },
            _ => NoContent()
        );
    }
}