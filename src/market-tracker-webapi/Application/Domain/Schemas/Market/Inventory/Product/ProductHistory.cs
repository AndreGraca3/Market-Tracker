namespace market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;

public record ProductHistory(string ProductId, List<ProductHistoryPrice> History, int NumberOfListPresent);

public record ProductHistoryPrice(
    DateTime Date,
    int Price
);
