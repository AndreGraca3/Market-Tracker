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
        Optional<bool> newIsFavorite,
        Optional<ProductReviewInputModel?> newReview
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

            var review = oldPreferences.Review;

            if (newReview.HasValue)
            {
                if (newReview.Value is not null)
                {
                    if (review is null)
                    {
                        var newReviewId = await productFeedbackRepository.AddReviewAsync(
                            clientId,
                            productId,
                            newReview.Value.Rating,
                            newReview.Value.Comment
                        );

                        review = await productFeedbackRepository.GetReviewByIdAsync(
                            newReviewId.Value
                        );
                    }
                    else
                    {
                        review = await productFeedbackRepository.UpdateReviewAsync(
                            review.Id.Value,
                            newReview.Value.Rating,
                            newReview.Value.Comment
                        );
                    }
                }
                else
                {
                    if (review is not null)
                        await productFeedbackRepository.RemoveReviewAsync(review.Id.Value);
                    review = null;
                }
            }

            var isFavourite = oldPreferences.IsFavourite;

            if (newIsFavorite.HasValue)
            {
                isFavourite = await productFeedbackRepository.UpdateProductFavouriteAsync(
                    clientId,
                    productId,
                    newIsFavorite.Value
                );
            }

            return new ProductPreferences(isFavourite, review);
        });
    }

    public async Task<IEnumerable<ProductItem>> GetFavouritesAsync(Guid clientId)
    {
        return await transactionManager.ExecuteAsync(async () =>
            await productFeedbackRepository.GetFavouriteProductsAsync(clientId));
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

    public async Task<ProductHistory> GetProductHistoryFromStoreByIdAsync(string productId, int storeId)
    {
        return await transactionManager.ExecuteAsync(async () =>
            await productFeedbackRepository.GetProductHistoryFromStoreByIdAsync(productId, storeId) ??
            throw new MarketTrackerServiceException(
                new ProductFetchingError.ProductByIdNotFound(productId)
            ));
    }
}