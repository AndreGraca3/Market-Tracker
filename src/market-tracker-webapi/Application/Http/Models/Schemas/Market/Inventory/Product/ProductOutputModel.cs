using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Category;
using Microsoft.OpenApi.Extensions;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product;

using Product = Domain.Schemas.Market.Inventory.Product.Product;

public record ProductOutputModel(
    string Id,
    string Name,
    string ImageUrl,
    int Quantity,
    string Unit,
    Brand Brand,
    CategoryOutputModel Category
);

public static class ProductInfoOutputModelMapper
{
    public static ProductOutputModel ToOutputModel(this Product product)
    {
        return new ProductOutputModel(
            product.Id.Value,
            product.Name,
            product.ImageUrl,
            product.Quantity,
            product.Unit.GetDisplayName(),
            product.Brand,
            product.Category.ToOutputModel()
        );
    }
}
