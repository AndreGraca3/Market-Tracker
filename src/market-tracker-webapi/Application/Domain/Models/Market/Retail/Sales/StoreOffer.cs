using market_tracker_webapi.Application.Domain.Models.Market.Retail.Sales.Pricing;
using market_tracker_webapi.Application.Domain.Models.Market.Retail.Shop;

namespace market_tracker_webapi.Application.Domain.Models.Market.Retail.Sales;

public record StoreOffer(Store Store, Price PriceData);