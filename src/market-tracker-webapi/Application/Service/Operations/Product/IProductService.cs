using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Product;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Product;

public interface IProductService
{
    public Task<Either<IServiceError, CollectionOutputModel>> GetProductsAsync();
    public Task<Either<ProductFetchingError, ProductOutputModel>> GetProductByIdAsync(
        string productId
    );

    public Task<Either<IServiceError, StringIdOutputModel>> AddProductAsync(
        string productId,
        string name,
        string imageUrl,
        int quantity,
        string unit,
        string brandName,
        int categoryId
    // int price
    );

    public Task<Either<IServiceError, ProductInfoOutputModel>> UpdateProductAsync(
        string productId,
        string name,
        string imageUrl,
        int quantity,
        string unit,
        string brandName,
        int categoryId
    );

    public Task<Either<ProductFetchingError, StringIdOutputModel>> RemoveProductAsync(
        string productId
    );
}
