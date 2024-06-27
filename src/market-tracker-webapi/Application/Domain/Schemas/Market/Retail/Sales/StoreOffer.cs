using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales.Pricing;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;

namespace market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales;

public record StoreOffer(Store Store, Price PriceData, StoreAvailability StoreAvailability);