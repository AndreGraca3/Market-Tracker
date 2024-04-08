using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Price;
using market_tracker_webapi.Application.Http.Models.Product;
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
    public async Task<Either<IServiceError, CollectionOutputModel>> GetProductsAsync()
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var products = await productRepository.GetProductsAsync();
            return EitherExtensions.Success<IServiceError, CollectionOutputModel>(
                new CollectionOutputModel(products)
            );
        });
    }

    public async Task<Either<ProductFetchingError, ProductOutputModel>> GetProductByIdAsync(
        string productId
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var product = await productRepository.GetProductByIdAsync(productId);
            if (product is null)
            {
                return EitherExtensions.Failure<ProductFetchingError, ProductOutputModel>(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            var brand = (await brandRepository.GetBrandByIdAsync(product.BrandId))!;
            var category = (await categoryRepository.GetCategoryByIdAsync(product.CategoryId))!;

            int? minPrice = null;
            int? maxPrice = null;

            var storesAvailability = await priceRepository.GetStoresAvailabilityByProductIdAsync(
                product.Id,
                DateTime.Now
            );

            var companyStoresDictionary = new Dictionary<int, List<StorePriceOutputModel>>();
            var companiesDictionary = new Dictionary<int, Domain.Company>();

            foreach (var storeAvailability in storesAvailability)
            {
                var storePrice = await priceRepository.GetStorePriceByProductIdAsync(
                    product.Id,
                    storeAvailability.StoreId,
                    DateTime.Now
                );

                if (!companyStoresDictionary.ContainsKey(storePrice.Store.Company.Id))
                {
                    companyStoresDictionary[storePrice.Store.Company.Id] =
                        new List<StorePriceOutputModel>();
                }

                if (minPrice is null || storePrice.PriceDetails.Price < minPrice)
                {
                    minPrice = storePrice.PriceDetails.Price;
                }

                if (maxPrice is null || storePrice.PriceDetails.Price > maxPrice)
                {
                    maxPrice = storePrice.PriceDetails.Price;
                }

                companyStoresDictionary[storePrice.Store.Company.Id]
                    .Add(
                        StorePriceOutputModel.ToStorePriceOutputModel(
                            Domain.Store.ToStore(storePrice.Store),
                            storePrice.Store.City,
                            storePrice.PriceDetails.Price,
                            storePrice.PriceDetails.Promotion,
                            storeAvailability.IsAvailable,
                            storeAvailability.LastChecked
                        )
                    );
                companiesDictionary[storePrice.Store.Company.Id] = storePrice.Store.Company;
            }

            return EitherExtensions.Success<ProductFetchingError, ProductOutputModel>(
                ProductOutputModel.ToProductOutputModel(
                    product,
                    brand,
                    category,
                    companyStoresDictionary
                        .Select(cStoresPrices => new CompanyPricesOutputModel(
                            cStoresPrices.Key,
                            companiesDictionary[cStoresPrices.Key].Name,
                            cStoresPrices.Value
                        ))
                        .ToList(),
                    minPrice,
                    maxPrice
                )
            );
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
