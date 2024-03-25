using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Service;
using market_tracker_webapi.Application.Service.Errors.Category;
using market_tracker_webapi.Application.Service.Errors.Product;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers;

public class ProductController(ProductService productService) : ControllerBase
{
    [HttpGet(Uris.Products.Base)]
    public async Task<ActionResult<List<ProductOutputModel>>> GetProductsAsync()
    {
        var products = await productService.GetProductsAsync();
        return Ok(products);
    }

    [HttpGet(Uris.Products.ProductById)]
    public async Task<ActionResult<ProductOutputModel>> GetProductAsync(int id)
    {
        var res = await productService.GetProductAsync(id);

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
    public async Task<ActionResult<IdOutputModel>> AddProductAsync(
        [FromBody] ProductCreationInputModel productInput
    )
    {
        var res = await productService.AddProductAsync(
            productInput.Id,
            productInput.Name,
            productInput.Description,
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
                    ProductCreationError.ProductAlreadyExists alreadyExists
                        => new ProductProblem.ProductAlreadyExists(alreadyExists).ToActionResult(),

                    ProductCreationError.InvalidBrand invalidBrand
                        => new ProductProblem.InvalidBrand(invalidBrand).ToActionResult(),

                    CategoryFetchingError.CategoryByIdNotFound categoryNotFound
                        => new CategoryProblem.CategoryByIdNotFound(
                            categoryNotFound
                        ).ToActionResult(),
                };
            }
        );
    }

    [HttpPut(Uris.Products.ProductById)]
    public async Task<ActionResult<ProductOutputModel>> UpdateProductAsync(
        int id,
        [FromBody] ProductCreationInputModel productInput
    )
    {
        throw new NotImplementedException();
    }

    [HttpDelete(Uris.Products.ProductById)]
    public async Task<ActionResult<IdOutputModel>> RemoveProductAsync(int id)
    {
        var res = await productService.RemoveProductAsync(id);

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
