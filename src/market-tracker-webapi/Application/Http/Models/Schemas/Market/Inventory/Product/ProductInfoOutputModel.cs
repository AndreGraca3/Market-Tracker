using market_tracker_webapi.Application.Domain.Models.Market.Inventory;
using Microsoft.OpenApi.Extensions;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product;

public record ProductInfoOutputModel(
    string ProductId,
    string Name,
    string ImageUrl,
    int Quantity,
    string Unit,
    Brand Brand,
    Domain.Models.Market.Inventory.Category Category
)
{
    public static ProductInfoOutputModel ToProductInfoOutputModel(
        Domain.Models.Market.Inventory.Product.Product product,
        Brand brand,
        Domain.Models.Market.Inventory.Category category
    )
    {
        return new ProductInfoOutputModel(
            product.Id,
            product.Name,
            product.ImageUrl,
            product.Quantity,
            product.Unit.GetDisplayName(),
            brand,
            category
        );
    }
}
