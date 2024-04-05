using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Operations.Product;

public interface IProductFeedbackRepository
{
    Task<IEnumerable<ProductReview>> GetReviewsByProductIdAsync(int productId);

    Task<int> AddReviewAsync(Guid clientId, int productId, int rate, string? comment);

    Task<ProductReview?> UpdateReviewAsync(Guid clientId, int productId, int rate, string? comment);

    Task<ProductReview?> RemoveReviewAsync(Guid clientId, int productId);

    Task<ProductPreferences> GetUserFeedbackByProductId(Guid clientId, int productId);

    public Task<ProductStats?> GetProductStatsByIdAsync(int productId);
}
