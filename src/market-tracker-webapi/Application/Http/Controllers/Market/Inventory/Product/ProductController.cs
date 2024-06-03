using market_tracker_webapi.Application.Domain.Filters.Product;
using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product.Offer;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Service.Operations.Market.Inventory.Product;
using market_tracker_webapi.Application.Service.Results;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Market.Inventory.Product;

[ApiController]
[Produces(Uris.JsonMediaType, Uris.JsonProblemMediaType)]
public class ProductController(IProductService productService, IProductPriceService productPriceService)
    : ControllerBase
{
    [HttpGet(Uris.Products.Base)]
    public async Task<ActionResult<PaginatedProductOffersOutputModel>> GetProductsAsync(
        [FromQuery] ProductsFiltersInputModel filters,
        [FromQuery] PaginationInputs paginationInputs,
        [FromQuery] ProductsSortOption? sortBy
    )
    {
        var paginatedProductOffers =
            await productService.GetBestAvailableProductsOffersAsync(
                paginationInputs.Skip,
                paginationInputs.ItemsPerPage,
                filters.MaxValuesPerFacet,
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
        return paginatedProductOffers.ToOutputModel();
    }

    [HttpGet(Uris.Products.ProductById)]
    public async Task<ActionResult<ProductOutputModel>> GetProductByIdAsync(string productId)
    {
        return (await productService.GetProductByIdAsync(productId)).ToOutputModel();
    }

    [HttpGet(Uris.Products.PricesByProductId)]
    public async Task<ActionResult<CompaniesPricesResultOutputModel>> GetProductPricesAsync(
        string productId)
    {
        var companiesPricesResult = await productPriceService.GetProductPricesAsync(productId);
        return companiesPricesResult.ToOutputModel();
    }

    [HttpPost(Uris.Products.Base)]
    [Authorized([Role.Operator])]
    public async Task<ActionResult<ProductCreationResult>> AddProductAsync(
        [FromBody] ProductCreationInputModel productInput
    )
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var productCreationResult = await productService.AddProductAsync(
            authUser.User.Id.Value,
            productInput.Id,
            productInput.Name,
            productInput.ImageUrl,
            productInput.Quantity,
            productInput.Unit,
            productInput.BrandName,
            productInput.CategoryId!.Value,
            productInput.BasePrice!.Value,
            productInput.PromotionPercentage
        );

        return productCreationResult.IsNew
            ? Created(Uris.Products.BuildProductByIdUri(productCreationResult.Id), productCreationResult)
            : productCreationResult;
    }

    [HttpPut(Uris.Products.AvailabilityByProductId)]
    [Authorized([Role.Operator])]
    public async Task<ActionResult> SetProductAvailabilityAsync(
        string productId, [FromBody] ProductAvailabilityInputModel availabilityInput
    )
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        await productService.SetProductAvailabilityAsync(
            authUser.User.Id.Value,
            productId,
            availabilityInput.IsAvailable
        );

        return NoContent();
    }

    [HttpPatch(Uris.Products.ProductById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<ProductOutputModel>> UpdateProductAsync(
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

        return res.ToOutputModel();
    }

    [HttpDelete(Uris.Products.ProductById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult> RemoveProductAsync(string productId)
    {
        await productService.RemoveProductAsync(productId);
        return NoContent();
    }
}