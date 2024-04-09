using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Dto.Product;

public record ProductDetails(
    string Id,
    string Name,
    string ImageUrl,
    int Quantity,
    string Unit,
    int Views,
    double Rating,
    Brand Brand,
    Category Category
)
{
    public static ProductDetails ToProductDetails(
        Domain.Product product,
        Brand brand,
        Category category
    )
    {
        return new ProductDetails(
            product.Id,
            product.Name,
            product.ImageUrl,
            product.Quantity,
            product.Unit,
            product.Views,
            product.Rating,
            brand,
            category
        );
    }
}
