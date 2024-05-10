using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Domain.Filters.Product;
using market_tracker_webapi.Application.Domain.Models.Market.Retail.Sales;
using market_tracker_webapi.Application.Domain.Models.Market.Retail.Sales.Pricing;
using market_tracker_webapi.Application.Domain.Models.Market.Retail.Shop;

namespace market_tracker_webapi.Application.Repository.Market.Price;

using Price = Domain.Models.Market.Retail.Sales.Pricing.Price;

public interface IPriceRepository
{
    /**
     *  Get the best offers products available in the market applying filters at current time.
     */
    public Task<PaginatedProductOffers> GetBestAvailableProductsOffersAsync(
        int skip,
        int take,
        int maxValuesPerFacet,
        ProductsSortOption? sortBy = null,
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