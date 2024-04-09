using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Dto.Product;

public record ProductInfo(
    string Id,
    string Name,
    string ImageUrl,
    int Quantity,
    string Unit,
    double Rating,
    Brand Brand,
    Category Category
)
{
    public static ProductInfo ToProductInfo(Domain.Product product, Brand brand, Category category)
    {
        return new ProductInfo(
            product.Id,
            product.Name,
            product.ImageUrl,
            product.Quantity,
            product.Unit,
            product.Rating,
            brand,
            category
        );
    }
    
    public static ProductInfo ToProductInfo(ProductDetails productDetails)
    {
        return new ProductInfo(
            productDetails.Id,
            productDetails.Name,
            productDetails.ImageUrl,
            productDetails.Quantity,
            productDetails.Unit,
            productDetails.Rating,
            productDetails.Brand,
            productDetails.Category
        );
    }
}
