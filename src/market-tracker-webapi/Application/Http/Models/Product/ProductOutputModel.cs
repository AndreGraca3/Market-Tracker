using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Http.Models;

public record ProductOutputModel(
    int Id,
    string Name,
    string ImageUrl,
    int Quantity,
    string Unit,
    int Views,
    float Rate,
    Brand Brand,
    Domain.Category Category
)
{
    public static ProductOutputModel ToProductOutputModel(
        Product product,
        Brand brand,
        Domain.Category category
    )
    {
        return new ProductOutputModel(
            product.Id,
            product.Name,
            product.ImageUrl,
            product.Quantity,
            product.Unit,
            product.Views,
            product.Rate,
            brand,
            category
        );
    }
}
