using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product.Feedback;
using market_tracker_webapi.Application.Repository.Market.Inventory.Product;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.Inventory.Product;

public class ProductFeedbackService(
    IProductRepository productRepository,
    IProductFeedbackRepository productFeedbackRepository,
    ITransactionManager transactionManager
) : IProductFeedbackService
{
    public async Task<PaginatedResult<ProductReview>>
        GetReviewsByProductIdAsync(string productId, int skip, int take)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await productRepository.GetProductByIdAsync(productId) is null)
                throw new MarketTrackerServiceException(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );

            return await productFeedbackRepository.GetReviewsByProductIdAsync(productId, skip, take);
        });
    }

    public async Task<ProductPreferences> UpsertProductPreferencesAsync(
        Guid clientId,
        string productId,
        Optional<bool> isFavorite,
        Optional<ProductReviewInputModel?> review
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await productRepository.GetProductByIdAsync(productId) is null)
            {
                throw new MarketTrackerServiceException(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            var oldPreferences = await productFeedbackRepository.GetProductPreferencesAsync(
                clientId, productId);

            var updatedReview = oldPreferences.Review;

            if (review.HasValue)
            {
                if (review.Value is not null)
                {
                    updatedReview = await productFeedbackRepository.UpsertReviewAsync(
                        clientId,
                        productId,
                        review.Value.Rating,
                        review.Value.Comment
                    );
                }
                else
                {
                    await productFeedbackRepository.RemoveReviewAsync(clientId, productId);
                    updatedReview = null;
                }
            }

            var updatedIsFavourite = oldPreferences.IsFavourite;

            if (isFavorite.HasValue)
            {
                updatedIsFavourite = await productFeedbackRepository.UpdateProductFavouriteAsync(
                    clientId,
                    productId,
                    isFavorite.Value
                );
            }

            return new ProductPreferences(updatedIsFavourite, updatedReview);
        });
    }

    public async Task<ProductPreferences> GetProductPreferencesAsync(Guid clientId,
        string productId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await productRepository.GetProductByIdAsync(productId) is null)
                throw new MarketTrackerServiceException(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );

            return await productFeedbackRepository.GetProductPreferencesAsync(
                clientId,
                productId
            );
        });
    }

    public async Task<ProductStats> GetProductStatsByIdAsync(string productId)
    {
        return await transactionManager.ExecuteAsync(async () =>
            await productFeedbackRepository.GetProductStatsByIdAsync(productId) ??
            throw new MarketTrackerServiceException(
                new ProductFetchingError.ProductByIdNotFound(productId)
            ));
    }
}