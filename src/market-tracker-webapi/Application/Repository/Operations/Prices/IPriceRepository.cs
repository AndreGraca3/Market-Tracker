using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Price;
using market_tracker_webapi.Application.Repository.Dto.Store;

namespace market_tracker_webapi.Application.Repository.Operations.Prices;

public interface IPriceRepository
{
    public Task<StorePrice?> GetCheapestStorePriceByProductIdAsync(
        string productId,
        DateTime priceAt,
        IList<int>? companyIds = null
    );

    public Task<IEnumerable<StoreAvailability>> GetStoresAvailabilityAsync(string productId);

    public Task<StoreAvailability?> GetStoreAvailabilityAsync(string productId, int storeId);

    public Task<StorePrice> GetStorePriceByProductIdAsync(
        string productId,
        int storeId,
        DateTime priceAt
    );

    public Task<IEnumerable<PriceInfo>> GetPriceHistoryByProductIdAndStoreIdAsync(
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