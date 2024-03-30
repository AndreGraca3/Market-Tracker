using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Product;

using Product = market_tracker_webapi.Application.Domain.Product;

public interface IProductService
{
    public Task<IEnumerable<Product>> GetProductsAsync();
    public Task<Either<ProductFetchingError, ProductOutputModel>> GetProductAsync(int id);

    public Task<Either<IServiceError, IdOutputModel>> AddProductAsync(
        int productId,
        string name,
        string imageUrl,
        int quantity,
        string unit,
        string brandName,
        int categoryId
    );

    public Task<Either<IServiceError, IdOutputModel>> UpdateProductAsync(
        int id,
        string? name,
        string? imageUrl,
        int? quantity,
        string? unit,
        string? brandName,
        int? categoryId
    );

    public Task<Either<ProductFetchingError, IdOutputModel>> RemoveProductAsync(int productId);
}
