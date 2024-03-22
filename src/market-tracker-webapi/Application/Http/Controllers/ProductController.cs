using market_tracker_webapi.Application.Http;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Service;
using market_tracker_webapi.Application.Service.Errors.Product;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Controllers;

public class ProductController(ProductService productService, ILogger<ProductController> logger)
    : ControllerBase
{
    [HttpGet(Uris.Products.ProductById)]
    public async Task<ActionResult<ProductOutputModel>> GetProductAsync(int id)
    {
        var res = await productService.GetProductAsync(id);
        if (res.IsSuccessful())
            return Ok(res.Value);
        switch (res.Error)
        {
            case ProductFetchingError.ProductByIdNotFound error:
                return NotFound(new ProductProblem.ProductByIdNotFound(error));

            default:
                throw new NotImplementedException();
        }
    }

    [HttpPost(Uris.Products.Base)]
    public async Task<ActionResult<IdOutputModel>> AddProductAsync(
        [FromBody] ProductCreationInputModel productInput
    )
    {
        throw new NotImplementedException();
    }
}
