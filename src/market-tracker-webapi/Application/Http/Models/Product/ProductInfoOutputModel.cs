using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Http.Models.Product;

public record ProductInfoOutputModel(
    string ProductId,
    string Name,
    string ImageUrl,
    int Quantity,
    string Unit,
    Brand Brand,
    Domain.Category Category
)
{
    public static ProductInfoOutputModel ToProductInfoOutputModel(
        Domain.Product product,
        Brand brand,
        Domain.Category category
    )
    {
        return new ProductInfoOutputModel(
            product.Id,
            product.Name,
            product.ImageUrl,
            product.Quantity,
            product.Unit,
            brand,
            category
        );
    }
}
