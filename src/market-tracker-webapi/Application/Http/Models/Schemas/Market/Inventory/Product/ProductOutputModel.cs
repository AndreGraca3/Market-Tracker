using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Category;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product;

using Product = Domain.Schemas.Market.Inventory.Product.Product;

public record ProductOutputModel(
    string Id,
    string Name,
    string ImageUrl,
    int Quantity,
    string Unit,
    double Rating,
    Brand Brand,
    CategoryOutputModel Category
);

public record ProductItemOutputModel(string ProductId, string Name, string ImageUrl, string BrandName);

public static class ProductInfoOutputModelMapper
{
    public static ProductOutputModel ToOutputModel(this Product product)
    {
        return new ProductOutputModel(
            product.Id.Value,
            product.Name,
            product.ImageUrl,
            product.Quantity,
            product.Unit.GetUnitName(),
            product.Rating,
            product.Brand,
            product.Category.ToOutputModel()
        );
    }
    
    public static ProductItemOutputModel ToProductItemOutputModel(this Product product)
    {
        return new ProductItemOutputModel(
            product.Id.Value,
            product.Name,
            product.ImageUrl,
            product.Brand.Name
        );
    }
}
