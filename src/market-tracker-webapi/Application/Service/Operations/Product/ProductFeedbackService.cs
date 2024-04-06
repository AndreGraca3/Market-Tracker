using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Product;
using market_tracker_webapi.Application.Http.Models.User;
using market_tracker_webapi.Application.Repository.Operations.Product;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Product;

public class ProductFeedbackService(
    IProductRepository productRepository,
    IProductFeedbackRepository productFeedbackRepository,
    ITransactionManager transactionManager
) : IProductFeedbackService
{
    private IProductFeedbackService _productFeedbackServiceImplementation;

    public async Task<
        Either<ProductFetchingError, CollectionOutputModel>
    > GetReviewsByProductIdAsync(int productId)
    {
        throw new NotImplementedException();
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await productRepository.GetProductByIdAsync(productId) is null)
            {
                return EitherExtensions.Failure<ProductFetchingError, CollectionOutputModel>(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            var reviews = await productFeedbackRepository.GetReviewsByProductIdAsync(productId);

            return EitherExtensions.Success<ProductFetchingError, CollectionOutputModel>(
                new CollectionOutputModel(
                    reviews.Select(review =>
                    {
                        // TODO: decide if its preferable to build this in repo
                        // TODO: get actual user
                        var user = new UserInfoOutputModel(review.ClientId, "dummy", "dummy");
                        return ProductReviewOutputModel.ToProductReviewOutputModel(user, review);
                    })
                )
            );
        });
    }

    public async Task<Either<IServiceError, IdOutputModel>> UpsertProductPreferencesAsync(
        Guid clientId,
        int productId,
        Optional<bool> isFavorite,
        Optional<PriceAlertInputModel?> priceAlert,
        Optional<ProductReviewInputModel?> review
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            // TODO: search if client exists

            if (await productRepository.GetProductByIdAsync(productId) is null)
            {
                return EitherExtensions.Failure<IServiceError, IdOutputModel>(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            var updatedReview = await productFeedbackRepository.UpdateReviewAsync(
                clientId,
                productId,
                1,
                "comment"
            );
            if (updatedReview is not null)
            {
                return EitherExtensions.Success<IServiceError, IdOutputModel>(
                    new IdOutputModel(updatedReview.Id)
                );
            }

            var newReviewId = await productFeedbackRepository.AddReviewAsync(
                clientId,
                productId,
                1,
                "comment"
            );
            return EitherExtensions.Success<IServiceError, IdOutputModel>(
                new IdOutputModel(newReviewId)
            );
        });
    }

    public async Task<Either<IServiceError, ProductPreferences>> GetUserFeedbackByProductId(
        Guid clientId,
        int productId
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            throw new NotImplementedException(); // TODO: search if client exists

            if (await productRepository.GetProductByIdAsync(productId) is null)
            {
                return EitherExtensions.Failure<IServiceError, ProductPreferences>(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            var clientFeedback = await productFeedbackRepository.GetUserFeedbackByProductId(
                clientId,
                productId
            );

            return EitherExtensions.Success<IServiceError, ProductPreferences>(clientFeedback);
        });
    }

    public async Task<Either<ProductFetchingError, ProductStats>> GetProductStatsByIdAsync(
        int productId
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var stats = await productFeedbackRepository.GetProductStatsByIdAsync(productId);

            if (stats is null)
            {
                return EitherExtensions.Failure<ProductFetchingError, ProductStats>(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            return EitherExtensions.Success<ProductFetchingError, ProductStats>(stats);
        });
    }
}
