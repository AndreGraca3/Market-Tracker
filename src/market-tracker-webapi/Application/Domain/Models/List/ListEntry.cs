using market_tracker_webapi.Application.Domain.Models.Market.Inventory.Product;

namespace market_tracker_webapi.Application.Domain.Models.List;

public record ListEntry(ListEntryId Id, Product Product, int? StoreId, int Quantity); 

public record ListEntryOffer(ProductOffer ProductOffer, int Quantity);

public record ListEntryId(
    string Value
);