using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
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
    public async Task<Either<IServiceError, CollectionOutputModel>> GetReviewsByProductIdAsync(
        int productId
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await productRepository.GetProductByIdAsync(productId) is null)
            {
                return EitherExtensions.Failure<IServiceError, CollectionOutputModel>(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            var reviews = await productFeedbackRepository.GetReviewsByProductIdAsync(productId);
            return EitherExtensions.Success<IServiceError, CollectionOutputModel>(
                new CollectionOutputModel(reviews)
            );
        });
    }

    public async Task<Either<ProductFetchingError, IdOutputModel>> UpsertReviewAsync(
        Guid clientId,
        int productId,
        int rate,
        string? comment
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await productRepository.GetProductByIdAsync(productId) is null)
            {
                return EitherExtensions.Failure<ProductFetchingError, IdOutputModel>(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            var updatedReview = await productFeedbackRepository.UpdateReviewAsync(
                clientId,
                productId,
                rate,
                comment
            );
            if (updatedReview is not null)
            {
                return EitherExtensions.Success<ProductFetchingError, IdOutputModel>(
                    new IdOutputModel(updatedReview.Id)
                );
            }

            var newReviewId = await productFeedbackRepository.AddReviewAsync(
                clientId,
                productId,
                rate,
                comment
            );
            return EitherExtensions.Success<ProductFetchingError, IdOutputModel>(
                new IdOutputModel(newReviewId)
            );
        });
    }

    public async Task<Either<ProductFetchingError, ProductPreferences>> GetUserFeedbackByProductId(
        Guid clientId,
        int productId
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await productRepository.GetProductByIdAsync(productId) is null)
            {
                return EitherExtensions.Failure<ProductFetchingError, ProductPreferences>(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            var clientFeedback = await productFeedbackRepository.GetUserFeedbackByProductId(
                clientId,
                productId
            );

            return EitherExtensions.Success<ProductFetchingError, ProductPreferences>(
                clientFeedback
            );
        });
    }
}
