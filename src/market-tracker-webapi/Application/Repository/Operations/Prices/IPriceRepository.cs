using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Price;
using market_tracker_webapi.Application.Repository.Dto.Product;
using market_tracker_webapi.Application.Repository.Dto.Store;

namespace market_tracker_webapi.Application.Repository.Operations.Prices;

public interface IPriceRepository
{
    /**
     *  Get the best offers products available in the market applying filters at current time.
     */
    public Task<PaginatedProductOffers> GetAvailableProductsOffersAsync(
        int skip,
        int take,
        SortByType? sortBy = null,
        string? name = null,
        IList<int>? categoryIds = null,
        IList<int>? brandIds = null,
        int? minPrice = null,
        int? maxPrice = null,
        int? minRating = null,
        int? maxRating = null,
        IList<int>? companyIds = null,
        IList<int>? storeIds = null,
        IList<int>? cityIds = null
    );

    /**
     * Get the cheapest store price available for a product at a given time.
     */
    public Task<StorePrice?> GetCheapestStorePriceAvailableByProductIdAsync(
        string productId,
        DateTime priceAt,
        IList<int>? companyIds = null,
        IList<int>? storeIds = null,
        IList<int>? cityIds = null
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

    public Task<string> AddPriceAsync(
        string productId,
        int storeId,
        int price,
        DateTime createdAt,
        int? promotionPercentage
    );
}