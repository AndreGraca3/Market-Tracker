using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Operations.Product;

public interface IProductFeedbackRepository
{
    Task<IEnumerable<ProductReview>> GetReviewsByProductIdAsync(int productId);

    Task<ProductReview?> UpsertReviewAsync(
        Guid clientId,
        int productId,
        int rating,
        string? comment
    );

    Task<ProductReview?> RemoveReviewAsync(Guid clientId, int productId);

    Task<PriceAlert> UpsertPriceAlertAsync(Guid clientId, int productId, int priceThreshold);

    Task<PriceAlert?> RemovePriceAlertAsync(Guid clientId, int productId);

    Task<bool> UpdateProductFavouriteAsync(Guid clientId, int productId, bool isFavourite);

    Task<ProductPreferences> GetUserFeedbackByProductId(Guid clientId, int productId);

    public Task<ProductStats?> GetProductStatsByIdAsync(int productId);
}
