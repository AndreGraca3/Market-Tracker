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

    public async Task<Either<ProductFetchingError, ProductOutputModel>> GetProductByIdAsync(int id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var product = await productRepository.GetProductByIdAsync(id);
            if (product is null)
            {
                return EitherExtensions.Failure<ProductFetchingError, ProductOutputModel>(
                    new ProductFetchingError.ProductByIdNotFound(id)
                );
            }

            var brand = (await brandRepository.GetBrandByIdAsync(product.BrandId))!;
            var category = (await categoryRepository.GetCategoryByIdAsync(product.CategoryId))!;

            var minPrice = int.MaxValue;
            var maxPrice = 0;

            var storesPrices = await priceRepository.GetStoresAvailabilityByProductIdAsync(
                product.Id,
                DateTime.Now
            );

            var companiesDictionary = new Dictionary<int, List<StorePrice>>();
            var companiesPrices = new List<CompanyPricesOutputModel>();

            foreach (var storePrice in storesPrices)
            {
                if (!companiesDictionary.ContainsKey(storePrice.Store.Company.Id))
                {
                    companiesDictionary[storePrice.Store.Company.Id] = new List<StorePrice>();
                }

                companiesDictionary[storePrice.Store.Company.Id].Add(storePrice);
            }

            foreach (var (_, prices) in companiesDictionary)
            {
                var company = prices.First().Store.Company;

                var minPriceForCompany = prices.Min(price => price.PriceDetails.Price);
                var maxPriceForCompany = prices.Max(price => price.PriceDetails.Price);

                if (minPriceForCompany < minPrice)
                {
                    minPrice = minPriceForCompany;
                }

                if (maxPriceForCompany > maxPrice)
                {
                    maxPrice = maxPriceForCompany;
                }

                companiesPrices.Add(
                    new CompanyPricesOutputModel(
                        company.Id,
                        company.Name,
                        prices
                            .Select(storePrice =>
                                StorePriceOutputModel.ToStorePriceOutputModel(
                                    Domain.Store.ToStore(storePrice.Store),
                                    storePrice.Store.City,
                                    storePrice.PriceDetails.Price,
                                    storePrice.PriceDetails.Promotion,
                                    true,
                                    DateTime.Now
                                )
                            )
                            .ToList()
                    )
                );
            }

            return EitherExtensions.Success<ProductFetchingError, ProductOutputModel>(
                ProductOutputModel.ToProductOutputModel(
                    product,
                    brand,
                    category,
                    companiesPrices,
                    minPrice,
                    maxPrice
                )
            );
        });
    }

    public async Task<Either<IServiceError, IdOutputModel>> AddProductAsync(
        int productId,
        string name,
        string imageUrl,
        int quantity,
        string unit,
        string brandName,
        int categoryId
    // int price
    )
    {
        // TODO: Add price in the name of operator who is adding the product
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await productRepository.GetProductByIdAsync(productId) is not null)
            {
                // TODO: Add new price entry and return
                return EitherExtensions.Failure<IServiceError, IdOutputModel>(
                    new ProductCreationError.ProductAlreadyExists(productId)
                );
            }

            var category = await categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category is null)
            {
                return EitherExtensions.Failure<IServiceError, IdOutputModel>(
                    new CategoryFetchingError.CategoryByIdNotFound(categoryId)
                );
            }

            var brand =
                await brandRepository.GetBrandByNameAsync(brandName)
                ?? await brandRepository.AddBrandAsync(brandName);

            await productRepository.AddProductAsync(
                productId,
                $"{brandName} {name} {quantity} {unit}",
                imageUrl,
                quantity,
                unit,
                brand.Id,
                categoryId
            );

            return EitherExtensions.Success<IServiceError, IdOutputModel>(
                new IdOutputModel(productId)
            );
        });
    }

    public async Task<Either<IServiceError, ProductInfoOutputModel>> UpdateProductAsync(
        int productId,
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

    public async Task<Either<ProductFetchingError, IdOutputModel>> RemoveProductAsync(int productId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var removedProduct = await productRepository.RemoveProductAsync(productId);
            if (removedProduct is null)
            {
                return EitherExtensions.Failure<ProductFetchingError, IdOutputModel>(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            return EitherExtensions.Success<ProductFetchingError, IdOutputModel>(
                new IdOutputModel(removedProduct.Id)
            );
        });
    }
}
