namespace market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales.Pricing;

public record PriceAlert(
    PriceAlertId Id,
    Guid ClientId,
    string ProductId,
    int StoreId,
    int PriceThreshold,
    DateTime CreatedAt)
{
    public PriceAlert(
        string id,
        Guid clientId,
        string productId,
        int storeId,
        int priceThreshold,
        DateTime createdAt
    ) : this(
        new PriceAlertId(id),
        clientId,
        productId,
        storeId,
        priceThreshold,
        createdAt
    )
    {
    }
};

public record PriceAlertId(
    string Value
);