using market_tracker_webapi.Application.Domain.Models.Market.Retail.Sales;
using market_tracker_webapi.Application.Domain.Models.Market.Store;

namespace market_tracker_webapi.Application.Domain.Models.Market.Inventory.Product;

public record ProductOffer(Product Product, StorePrice? StorePrice, bool IsAvailable);