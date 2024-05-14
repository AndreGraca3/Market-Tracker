using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales.Pricing;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;

namespace market_tracker_webapi.Application.Service.Results;

public record CompaniesPricesResult(
    IEnumerable<CompanyPrices> Companies,
    int MinPrice,
    int MaxPrice
);

public record CompanyPrices(
    int Id,
    string Name,
    IEnumerable<StoreOfferResult> Stores
);

public record StoreOfferResult(Store Store, Price PriceData, bool IsCheapestOffer);