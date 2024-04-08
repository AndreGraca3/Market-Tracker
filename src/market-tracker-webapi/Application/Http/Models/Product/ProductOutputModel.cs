using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models.Price;

namespace market_tracker_webapi.Application.Http.Models.Product;

public record ProductOutputModel(
    ProductInfoOutputModel Product,
    List<CompanyPricesOutputModel> Companies,
    int? MinPrice,
    int? MaxPrice
)
{
    public static ProductOutputModel ToProductOutputModel(
        Domain.Product product,
        Brand brand,
        Domain.Category category,
        List<CompanyPricesOutputModel> companies,
        int? minPrice,
        int? maxPrice
    )
    {
        return new ProductOutputModel(
            new ProductInfoOutputModel(
                product.Id,
                product.Name,
                product.ImageUrl,
                product.Quantity,
                product.Unit,
                brand,
                category
            ),
            companies,
            minPrice,
            maxPrice
        );
    }
}
