using market_tracker_webapi.Application.Service.Results;

namespace market_tracker_webapi.Application.Service.Operations.Market.Inventory.Product;

public interface IProductPriceService
{
    public Task<IEnumerable<CompanyPricesResult>> GetProductPricesAsync(string productId);

    // TODO: price history
}