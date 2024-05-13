using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;

namespace market_tracker_webapi.Application.Domain.Schemas.List;

public record ListEntry(ListEntryId Id, Product Product, int? StoreId, int Quantity); 

public record ListEntryOffer(ProductOffer ProductOffer, int Quantity);

public record ListEntryId(
    string Value
);