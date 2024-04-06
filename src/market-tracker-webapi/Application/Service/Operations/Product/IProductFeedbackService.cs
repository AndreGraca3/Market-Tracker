using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Product;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Product;

public interface IProductFeedbackService
{
    Task<Either<ProductFetchingError, CollectionOutputModel>> GetReviewsByProductIdAsync(
        int productId
    );

    Task<Either<IServiceError, ProductPreferences>> UpsertProductPreferencesAsync(
        Guid clientId,
        int productId,
        Optional<bool> isFavorite,
        Optional<PriceAlertInputModel?> priceAlert,
        Optional<ProductReviewInputModel?> review
    );

    Task<Either<IServiceError, ProductPreferences>> GetUserFeedbackByProductId(
        Guid clientId,
        int productId
    );

    Task<Either<ProductFetchingError, ProductStats>> GetProductStatsByIdAsync(int productId);
}
