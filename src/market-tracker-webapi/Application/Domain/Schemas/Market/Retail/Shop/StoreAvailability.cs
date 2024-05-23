namespace market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;

public record StoreAvailability(
    int StoreId,
    string ProductId,
    bool IsAvailable,
    DateTime LastChecked
);
