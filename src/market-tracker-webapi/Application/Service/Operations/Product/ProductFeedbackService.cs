using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models.Product;
using market_tracker_webapi.Application.Http.Models.User;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Operations.Client;
using market_tracker_webapi.Application.Repository.Operations.Product;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Product;

public class ProductFeedbackService(
    IProductRepository productRepository,
    IProductFeedbackRepository productFeedbackRepository,
    IClientRepository clientRepository,
    ITransactionManager transactionManager
) : IProductFeedbackService
{
    public async Task<Either<ProductFetchingError, PaginatedResult<ProductReviewOutputModel>>>
        GetReviewsByProductIdAsync(string productId, int skip, int take)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await productRepository.GetProductByIdAsync(productId) is null)
            {
                return EitherExtensions.Failure<ProductFetchingError, PaginatedResult<ProductReviewOutputModel>>(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            var paginatedReviews =
                await productFeedbackRepository.GetReviewsByProductIdAsync(productId, skip, take);

            var clientReviewsTasks = paginatedReviews.Items.Select(async review =>
            {
                var client = await clientRepository.GetClientByIdAsync(review.ClientId);
                var user = new UserInfoOutputModel(review.ClientId, client!.Username, client.AvatarUrl);
                return ProductReviewOutputModel.ToProductReviewOutputModel(user, review);
            });

            var clientReviews = await Task.WhenAll(clientReviewsTasks);

            return EitherExtensions.Success<ProductFetchingError, PaginatedResult<ProductReviewOutputModel>>(
                new PaginatedResult<ProductReviewOutputModel>(
                    clientReviews,
                    paginatedReviews.TotalItems,
                    skip,
                    take
                )
            );
        });
    }

    public async Task<Either<IServiceError, ProductPreferences>> UpsertProductPreferencesAsync(
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
                return EitherExtensions.Failure<IServiceError, ProductPreferences>(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            var oldPreferences = await productFeedbackRepository.GetProductsPreferencesAsync(
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

            return EitherExtensions.Success<IServiceError, ProductPreferences>(
                new ProductPreferences(updatedIsFavourite, updatedReview)
            );
        });
    }

    public async Task<Either<IServiceError, ProductPreferences>> GetProductsPreferencesAsync(Guid clientId,
        string productId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await productRepository.GetProductByIdAsync(productId) is null)
            {
                return EitherExtensions.Failure<IServiceError, ProductPreferences>(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            var clientFeedback = await productFeedbackRepository.GetProductsPreferencesAsync(
                clientId,
                productId
            );

            return EitherExtensions.Success<IServiceError, ProductPreferences>(clientFeedback);
        });
    }

    public async Task<Either<ProductFetchingError, ProductStats>> GetProductStatsByIdAsync(string productId)
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