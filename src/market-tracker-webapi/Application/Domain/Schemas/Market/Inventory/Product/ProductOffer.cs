using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales;

namespace market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;

public record ProductOffer(Product Product, StoreOffer? StoreOffer, bool IsAvailable);