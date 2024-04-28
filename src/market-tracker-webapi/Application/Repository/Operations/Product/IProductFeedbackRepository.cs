using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repository.Dto;

namespace market_tracker_webapi.Application.Repository.Operations.Product;

public interface IProductFeedbackRepository
{
    Task<PaginatedResult<ProductReview>> GetReviewsByProductIdAsync(string productId, int skip, int take);

    Task<ProductReview> AddReviewAsync(
        Guid clientId,
        string productId,
        int rating,
        string? comment
    );

    Task<ProductReview?> UpsertReviewAsync(
        Guid clientId,
        string productId,
        int rating,
        string? comment
    );

    Task<ProductReview?> RemoveReviewAsync(Guid clientId, string productId);

    Task<PriceAlert> AddPriceAlertAsync(
        Guid clientId,
        string productId,
        int priceThreshold
    );

    Task<PriceAlert> UpsertPriceAlertAsync(Guid clientId, string productId, int priceThreshold);

    Task<PriceAlert?> RemovePriceAlertAsync(Guid clientId, string productId);

    Task<bool> UpdateProductFavouriteAsync(Guid clientId, string productId, bool isFavourite);

    Task<ProductPreferences> GetProductsPreferencesAsync(Guid clientId, string productId);

    public Task<ProductStats?> GetProductStatsByIdAsync(string productId);
}