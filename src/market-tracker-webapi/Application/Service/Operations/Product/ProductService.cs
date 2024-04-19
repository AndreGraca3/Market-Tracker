using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Price;
using market_tracker_webapi.Application.Http.Models.Product;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Product;
using market_tracker_webapi.Application.Repository.Operations.Brand;
using market_tracker_webapi.Application.Repository.Operations.Category;
using market_tracker_webapi.Application.Repository.Operations.Prices;
using market_tracker_webapi.Application.Repository.Operations.Product;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Category;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Product;

public class ProductService(
    IProductRepository productRepository,
    IBrandRepository brandRepository,
    ICategoryRepository categoryRepository,
    IPriceRepository priceRepository,
    ITransactionManager transactionManager
) : IProductService
{
    public async Task<Either<IServiceError, PaginatedProductsOutputModel>> GetProductsAsync(
        int skip,
        int take,
        string? searchName,
        IList<int>? categoryIds,
        IList<int>? brandIds,
        IList<int>? companyIds,
        int? minRating,
        int? maxRating
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var paginatedProducts = await productRepository.GetProductsAsync(
                skip,
                take,
                searchName,
                categoryIds,
                brandIds,
                minRating,
                maxRating
            );

            var productsOffers = new List<ProductOffer>();
            var hitsCounters = new ProductsHitsCounters();

            foreach (var product in paginatedProducts.Items)
            {
                var cheapestStorePrice =
                    await priceRepository.GetCheapestStorePriceByProductIdAsync(product.Id, DateTime.Now, companyIds);

                if (cheapestStorePrice is null)
                {
                    continue; // Skip product if no price is found
                }
                
                productsOffers.Add(new ProductOffer(product, cheapestStorePrice));

                var productBrandId = product.Brand.Id;
                var productCategoryId = product.Category.Id;
                var hasPromotion = cheapestStorePrice.PriceData.Promotion != null;

                hitsCounters.AddOrUpdateBrandHitsCounter(productBrandId, product.Brand.Name);
                hitsCounters.AddOrUpdateCategoryHitsCounter(productCategoryId, product.Category.Name);
                hitsCounters.AddOrUpdateCompanyHitsCounter(
                    cheapestStorePrice.Store.Company.Id,
                    cheapestStorePrice.Store.Company.Name
                );
                hitsCounters.AddOrUpdatePromotionHitsCounter(hasPromotion);
            }

            var paginatedProductOffers =
                new PaginatedResult<ProductOffer>(productsOffers, paginatedProducts.TotalItems, skip, take);

            return EitherExtensions.Success<IServiceError, PaginatedProductsOutputModel>(
                new PaginatedProductsOutputModel(paginatedProductOffers, hitsCounters)
            );
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

    public async Task<Either<IServiceError, StringIdOutputModel>> AddProductAsync(
        string productId,
        string name,
        string imageUrl,
        int quantity,
        string unit,
        string brandName,
        int categoryId
        // int price
    )
    {
        // TODO: Add price in the name of operator who is trying to add the product
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await productRepository.GetProductByIdAsync(productId) is not null)
            {
                // TODO: Add new price entry and return
                return EitherExtensions.Failure<IServiceError, StringIdOutputModel>(
                    new ProductCreationError.ProductAlreadyExists(productId)
                );
            }

            var category = await categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category is null)
            {
                return EitherExtensions.Failure<IServiceError, StringIdOutputModel>(
                    new CategoryFetchingError.CategoryByIdNotFound(categoryId)
                );
            }

            var brand =
                await brandRepository.GetBrandByNameAsync(brandName)
                ?? await brandRepository.AddBrandAsync(brandName);

            await productRepository.AddProductAsync(
                productId,
                $"{brandName} {name} {quantity}{unit.Substring(0, 2)}",
                imageUrl,
                quantity,
                unit,
                brand.Id,
                categoryId
            );

            return EitherExtensions.Success<IServiceError, StringIdOutputModel>(
                new StringIdOutputModel(productId)
            );
        });
    }

    public async Task<Either<IServiceError, ProductInfoOutputModel>> UpdateProductAsync(
        string productId,
        string name,
        string imageUrl,
        int quantity,
        string unit,
        string brandName,
        int categoryId
    )
    {
        // Moderators only
        return await transactionManager.ExecuteAsync(async () =>
        {
            var brand =
                await brandRepository.GetBrandByNameAsync(brandName)
                ?? await brandRepository.AddBrandAsync(brandName);

            var category = await categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category is null)
            {
                return EitherExtensions.Failure<IServiceError, ProductInfoOutputModel>(
                    new CategoryFetchingError.CategoryByIdNotFound(categoryId)
                );
            }

            var updatedProduct = await productRepository.UpdateProductAsync(
                productId,
                name,
                imageUrl,
                quantity,
                unit,
                brand.Id,
                category.Id
            );

            if (updatedProduct is null)
            {
                return EitherExtensions.Failure<IServiceError, ProductInfoOutputModel>(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            return EitherExtensions.Success<IServiceError, ProductInfoOutputModel>(
                ProductInfoOutputModel.ToProductInfoOutputModel(updatedProduct, brand, category)
            );
        });
    }

    public async Task<Either<ProductFetchingError, StringIdOutputModel>> RemoveProductAsync(
        string productId
    )
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