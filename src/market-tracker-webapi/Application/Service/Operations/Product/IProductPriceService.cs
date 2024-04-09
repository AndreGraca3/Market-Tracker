using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Product;

public interface IProductPriceService
{
    public Task<Either<ProductFetchingError, CollectionOutputModel>> GetProductPricesAsync(string productId);

    // TODO: price history
}