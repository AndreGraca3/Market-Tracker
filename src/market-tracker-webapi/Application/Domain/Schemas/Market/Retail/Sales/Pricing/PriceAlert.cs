namespace market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales.Pricing;

public record PriceAlert(
    PriceAlertId Id,
    Guid ClientId,
    PriceAlertProduct Product,
    PriceAlertStore Store,
    int PriceThreshold,
    DateTime CreatedAt
);

public record PriceAlertProduct(
    string Id,
    string Name,
    string ImageUrl
);

public record PriceAlertStore(
    int Id,
    string Name
);

public record PriceAlertId(
    string Value
);