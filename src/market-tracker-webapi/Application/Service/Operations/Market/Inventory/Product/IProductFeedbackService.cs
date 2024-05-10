using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Models.Market.Inventory.Product;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.Inventory.Product;

public interface IProductFeedbackService
{
    Task<Either<ProductFetchingError, PaginatedResult<ProductReview>>> GetReviewsByProductIdAsync(
        string productId,
        int skip,
        int take
    );

    Task<Either<IServiceError, ProductPreferences>> UpsertProductPreferencesAsync(
        Guid clientId,
        string productId,
        Optional<bool> isFavorite,
        Optional<ProductReviewInputModel?> review
    );

    Task<Either<IServiceError, ProductPreferences>> GetProductsPreferencesAsync(Guid clientId, string productId);

    Task<Either<ProductFetchingError, ProductStats>> GetProductStatsByIdAsync(string productId);
}