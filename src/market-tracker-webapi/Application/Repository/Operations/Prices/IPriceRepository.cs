using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Operations.Prices;

public interface IPriceRepository
{
    public Task<StorePrice> GetCheapestStorePriceByProductIdAsync(int productId, DateTime priceAt);

    public Task<IEnumerable<StorePrice>> GetStoresAvailabilityByProductIdAsync(
        int productId,
        DateTime date
    );

    public Task<StorePrice> GetStorePriceByProductIdAsync(
        int productId,
        int storeId,
        DateTime priceAt
    );

    public Task<IEnumerable<PriceEntry>> GetStorePricesHistoryByProductIdAndStoreIdAsync(
        int productId,
        int storeId,
        DateTime pricedAfter,
        DateTime pricedBefore
    );

    public Task AddPriceAsync(
        int productId,
        int storeId,
        int price,
        DateTime createdAt,
        int? promotionPercentage
    );
}
