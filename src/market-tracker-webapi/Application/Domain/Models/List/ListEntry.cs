using market_tracker_webapi.Application.Domain.Models.Market.Inventory.Product;

namespace market_tracker_webapi.Application.Domain.Models.List;

public record ListEntry(ProductOffer ProductOffer, int Quantity);