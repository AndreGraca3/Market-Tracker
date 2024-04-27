using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Product;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Product;
using market_tracker_webapi.Application.Repository.Operations.Brand;
using market_tracker_webapi.Application.Repository.Operations.Category;
using market_tracker_webapi.Application.Repository.Operations.Prices;
using market_tracker_webapi.Application.Repository.Operations.Product;
using market_tracker_webapi.Application.Repository.Operations.Store;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Category;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Product;

public class ProductService(
    IProductRepository productRepository,
    IBrandRepository brandRepository,
    ICategoryRepository categoryRepository,
    IPriceRepository priceRepository,
    IStoreRepository storeRepository,
    ITransactionManager transactionManager
) : IProductService
{
    public async Task<Either<IServiceError, PaginatedProductOffers>> GetBestAvailableProductsOffersAsync(int skip,
        int take,
        SortByType? sortBy, string? searchName, IList<int>? categoryIds, IList<int>? brandIds,
        IList<int>? companyIds, int? minPrice, int? maxPrice, int? minRating, int? maxRating)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var paginatedOffers = await priceRepository.GetBestAvailableProductsOffersAsync(
                skip,
                take,
                sortBy,
                searchName,
                categoryIds,
                brandIds,
                minPrice,
                maxPrice,
                minRating,
                maxRating,
                companyIds
            );

            return EitherExtensions.Success<IServiceError, PaginatedProductOffers>(paginatedOffers);
        });
    }

    public async Task<Either<ProductFetchingError, ProductInfo>> GetProductByIdAsync(string productId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var productDetails = await productRepository.GetProductByIdAsync(productId);
            if (productDetails is null)
            {
                return EitherExtensions.Failure<ProductFetchingError, ProductInfo>(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            return EitherExtensions.Success<ProductFetchingError, ProductInfo>(
                ProductInfo.ToProductInfo(productDetails));
        });
    }

    public async Task<Either<IServiceError, ProductCreationOutputModel>> AddProductAsync(
        AuthenticatedUser authUser,
        string productId,
        string name,
        string imageUrl,
        int quantity,
        ProductUnit unit,
        string brandName,
        int categoryId,
        int price,
        int? promotionPercentage
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var oldProduct = await productRepository.GetProductByIdAsync(productId);
            if (oldProduct is null)
            {
                if (await categoryRepository.GetCategoryByIdAsync(categoryId) is null)
                {
                    return EitherExtensions.Failure<IServiceError, ProductCreationOutputModel>(
                        new CategoryFetchingError.CategoryByIdNotFound(categoryId)
                    );
                }

                var brand = await brandRepository.GetBrandByNameAsync(brandName)
                            ?? await brandRepository.AddBrandAsync(brandName);

                await productRepository.AddProductAsync(
                    productId,
                    $"{brandName} {name}" + (quantity == 1 && unit == ProductUnit.Units
                        ? ""
                        : $" {quantity}{unit.GetBaseUnitName()}"),
                    imageUrl,
                    quantity,
                    unit.GetUnitName(),
                    brand.Id,
                    categoryId
                );
            }

            var store = await storeRepository.GetStoreByOperatorIdAsync(authUser.User.Id);

            if (store is null)
            {
                return EitherExtensions.Failure<IServiceError, ProductCreationOutputModel>(
                    new StoreFetchingError.StoreByOperatorIdNotFound(authUser.User.Id)
                );
            }

            var currentPrice = oldProduct is null
                ? null
                : await priceRepository.GetStorePriceAsync(productId, store.Id, DateTime.Now);

            var priceChanged = promotionPercentage is not null
                ? currentPrice?.PriceData.FinalPrice != price - price * promotionPercentage / 100
                : currentPrice?.PriceData.FinalPrice != price;

            if (priceChanged)
            {
                await priceRepository.AddPriceAsync(productId, store.Id, price, promotionPercentage);
            }

            return EitherExtensions.Success<IServiceError, ProductCreationOutputModel>(
                new ProductCreationOutputModel
                {
                    Id = productId,
                    IsNew = oldProduct is null,
                    PriceChanged = priceChanged
                });
        });
    }

    public async Task<Either<IServiceError, ProductInfoOutputModel>> UpdateProductAsync(
        string productId,
        string? name,
        string? imageUrl,
        int? quantity,
        string? unit,
        string? brandName,
        int? categoryId
    )
    {
        // Moderators only
        return await transactionManager.ExecuteAsync(async () =>
        {
            var product = await productRepository.GetProductByIdAsync(productId);
            if (product is null)
            {
                return EitherExtensions.Failure<IServiceError, ProductInfoOutputModel>(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            var brand = brandName is not null
                ? await brandRepository.GetBrandByNameAsync(brandName)
                  ?? await brandRepository.AddBrandAsync(brandName)
                : product.Brand;

            var category = product.Category;
            if (categoryId is not null)
            {
                category = await categoryRepository.GetCategoryByIdAsync(categoryId.Value);
                if (category is null)
                {
                    return EitherExtensions.Failure<IServiceError, ProductInfoOutputModel>(
                        new CategoryFetchingError.CategoryByIdNotFound(categoryId.Value)
                    );
                }
            }

            var updatedProduct = await productRepository.UpdateProductAsync(
                productId,
                name,
                imageUrl,
                quantity,
                unit,
                brand.Id,
                categoryId
            );

            return EitherExtensions.Success<IServiceError, ProductInfoOutputModel>(
                ProductInfoOutputModel.ToProductInfoOutputModel(updatedProduct!, brand, category)
            );
        });
    }

    public async Task<Either<ProductFetchingError, StringIdOutputModel>> RemoveProductAsync(string productId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var removedProduct = await productRepository.RemoveProductAsync(productId);
            if (removedProduct is null)
            {
                return EitherExtensions.Failure<ProductFetchingError, StringIdOutputModel>(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            return EitherExtensions.Success<ProductFetchingError, StringIdOutputModel>(
                new StringIdOutputModel(removedProduct.Id)
            );
        });
    }
}