using market_tracker_webapi.Application.Http.Models;
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
    public async Task<IEnumerable<Domain.Product>> GetProductsAsync()
    {
        var products = await productRepository.GetProductsAsync();
        return products;
    }

    public async Task<Either<ProductFetchingError, ProductOutputModel>> GetProductAsync(int id)
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
    }

    public async Task<Either<IServiceError, IdOutputModel>> AddProductAsync(
        int productId,
        string name,
        string imageUrl,
        int quantity,
        string unit,
        string brandName,
        int categoryId
    )
    {
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

    public async Task<Either<IServiceError, IdOutputModel>> UpdateProductAsync(
        int id,
        string? name,
        string? imageUrl,
        int? quantity,
        string? unit,
        string? brandName,
        int? categoryId
    )
    {
        throw new NotImplementedException();
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