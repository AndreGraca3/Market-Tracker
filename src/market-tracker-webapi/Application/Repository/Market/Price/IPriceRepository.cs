using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales.Pricing;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;

namespace market_tracker_webapi.Application.Repository.Market.Price;

using Price = Domain.Schemas.Market.Retail.Sales.Pricing.Price;

public interface IPriceRepository
{
    /**
     * Get the cheapest store offer available for a product at current time.
     */
    public Task<StoreOffer?> GetCheapestStoreOfferAvailableByProductIdAsync(
        string productId,
        IList<int>? companyIds = null,
        IList<int>? storeIds = null,
        IList<int>? cityIds = null
    );

    public Task<IEnumerable<StoreAvailability>> GetStoresAvailabilityAsync(string productId);

    public Task<StoreAvailability?> GetStoreAvailabilityStatusAsync(string productId, int storeId);

    /**
     * Get the store offer for a product at a specific time.
     */
    public Task<StoreOffer?> GetStoreOfferAsync(
        string productId,
        int storeId,
        DateTime priceAt
    );

    public Task<IEnumerable<Price>> GetPriceHistoryByProductIdAndStoreIdAsync(
        string productId,
        int storeId,
        DateTime pricedAfter,
        DateTime pricedBefore
    );

    public Task<PriceId> AddPriceAsync(
        string productId,
        int storeId,
        int price,
        int? promotionPercentage
    );
}