using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product.Feedback;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.Inventory.Product;

public interface IProductFeedbackService
{
    Task<PaginatedResult<ProductReview>> GetReviewsByProductIdAsync(
        string productId,
        int skip,
        int take
    );

    Task<ProductPreferences> UpsertProductPreferencesAsync(
        Guid clientId,
        string productId,
        Optional<bool> newIsFavorite,
        Optional<ProductReviewInputModel?> review
    );

    Task<IEnumerable<ProductItem>> GetFavouritesAsync(Guid clientId);

    Task<ProductPreferences> GetProductPreferencesAsync(Guid clientId, string productId);

    Task<ProductStats> GetProductStatsByIdAsync(string productId);

    Task<ProductHistory> GetProductHistoryFromStoreByIdAsync(string productId, int storeId);
}