using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Product;
using market_tracker_webapi.Application.Repository.Operations.Brand;
using market_tracker_webapi.Application.Repository.Operations.Category;
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

            return EitherExtensions.Success<ProductFetchingError, ProductOutputModel>(
                ProductOutputModel.ToProductOutputModel(product, brand, category)
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

    public async Task<Either<IServiceError, ProductOutputModel>> UpdateProductAsync(
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
                return EitherExtensions.Failure<IServiceError, ProductOutputModel>(
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
                return EitherExtensions.Failure<IServiceError, ProductOutputModel>(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            return EitherExtensions.Success<IServiceError, ProductOutputModel>(
                ProductOutputModel.ToProductOutputModel(updatedProduct, brand, category)
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
