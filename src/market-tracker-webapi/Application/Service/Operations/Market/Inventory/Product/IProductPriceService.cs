using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Price;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Results;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.Inventory.Product;

public interface IProductPriceService
{
    public Task<Either<ProductFetchingError, IEnumerable<CompanyPricesResult>>> GetProductPricesAsync(
        string productId);

    // TODO: price history
}