using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Operations.Prices;

public interface IPriceRepository
{
    public Task<StorePrice> GetCheapestStorePriceByProductIdAsync(
        string productId,
        DateTime priceAt
    );

    public Task<IEnumerable<StoreAvailability>> GetStoresAvailabilityByProductIdAsync(
        string productId,
        DateTime date
    );

    public Task<StorePrice> GetStorePriceByProductIdAsync(
        string productId,
        int storeId,
        DateTime priceAt
    );

    public Task<IEnumerable<PriceEntry>> GetPriceHistoryByProductIdAndStoreIdAsync(
        string productId,
        int storeId,
        DateTime pricedAfter,
        DateTime pricedBefore
    );

    public Task AddPriceAsync(
        string productId,
        int storeId,
        int price,
        DateTime createdAt,
        int? promotionPercentage
    );
}
