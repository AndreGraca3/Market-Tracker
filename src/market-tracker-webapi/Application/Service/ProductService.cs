using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository;
using market_tracker_webapi.Application.Repository.Interfaces;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;

namespace market_tracker_webapi.Application.Service;

public class ProductService(
    IProductRepository productRepository,
    TransactionManager transactionManager
)
{
    public async Task<Either<ProductFetchingError, ProductOutputModel>> GetProductAsync(int id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            ProductEntity? product = await productRepository.GetProductAsync(id);
            if (product is null)
            {
                return EitherExtensions.Failure<ProductFetchingError, ProductOutputModel>(
                    new ProductFetchingError.ProductByIdNotFound(id)
                );
            }

            return EitherExtensions.Success<ProductFetchingError, ProductOutputModel>(
                new ProductOutputModel(
                    product.Id,
                    product.Name,
                    product.Description,
                    product.ImageUrl,
                    product.Quantity,
                    product.Unit,
                    product.Views,
                    product.Rate,
                    1,
                    1
                )
            );
        });
    }

    public async Task<IdOutputModel> AddProductAsync(
        int id,
        string name,
        string description,
        int brandId,
        int categoryId
    )
    {
        throw new NotImplementedException();
        /*return await transactionManager.ExecuteAsync(async () =>
        {
            var productId = await productRepository.AddProductAsync(
                name,
                description,
                brandId,
                categoryId
            );
            return new ProductCreationOutputModel(productId);
        });*/
    }
}
