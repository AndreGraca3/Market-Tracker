using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product.Feedback;

public record ProductPreferencesOutputModel(bool IsFavourite, ProductReviewOutputModel? Review);

public static class ProductPreferencesOutputModelMapper
{
    public static ProductPreferencesOutputModel ToOutputModel(this ProductPreferences productPreferences)
    {
        return new ProductPreferencesOutputModel(
            productPreferences.IsFavourite,
            productPreferences.Review?.ToOutputModel()
        );
    }
}