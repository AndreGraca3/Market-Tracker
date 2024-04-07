namespace market_tracker_webapi.Application.Domain;

public record PriceEntry(
    int ProductId,
    int StoreId,
    int Price,
    Promotion Promotion,
    DateTime CreatedAt
);
