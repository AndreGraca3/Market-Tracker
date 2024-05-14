using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;

namespace market_tracker_webapi.Application.Repository.Market.Inventory.Product;

public interface IProductFeedbackRepository
{
    Task<PaginatedResult<ProductReview>> GetReviewsByProductIdAsync(string productId, int skip, int take);

    Task<ReviewId> AddReviewAsync(
        Guid clientId,
        string productId,
        int rating,
        string? comment
    );

    Task<ProductReview?> UpdateReviewAsync(
        Guid clientId,
        string productId,
        int rating,
        string? comment
    );

    Task<ProductReview?> RemoveReviewAsync(Guid clientId, string productId);

    Task<bool> UpdateProductFavouriteAsync(Guid clientId, string productId, bool isFavourite);

    Task<ProductPreferences> GetProductPreferencesAsync(Guid clientId, string productId);

    public Task<ProductStats?> GetProductStatsByIdAsync(string productId);
}