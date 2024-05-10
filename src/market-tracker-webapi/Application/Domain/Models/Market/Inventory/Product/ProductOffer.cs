using market_tracker_webapi.Application.Domain.Models.Market.Retail.Sales;

namespace market_tracker_webapi.Application.Domain.Models.Market.Inventory.Product;

public record ProductOffer(Product Product, StoreOffer? StoreOffer, bool IsAvailable);