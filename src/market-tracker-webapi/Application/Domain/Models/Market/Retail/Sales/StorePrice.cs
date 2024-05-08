namespace market_tracker_webapi.Application.Domain.Models.Market.Retail.Sales;

using Price = Pricing.Price;

public record StorePrice(Shop.Store Store, Price PriceData);