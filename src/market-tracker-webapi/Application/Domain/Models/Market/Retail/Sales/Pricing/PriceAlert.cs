namespace market_tracker_webapi.Application.Domain.Models.Market.Retail.Sales.Pricing;

public record PriceAlert(PriceAlertId Id, Guid ClientId, string ProductId, int PriceThreshold, DateTime CreatedAt)
{
    public PriceAlert(
        string Id,
        Guid ClientId,
        string ProductId,
        int PriceThreshold,
        DateTime CreatedAt
    ) : this(
        new PriceAlertId(Id),
        ClientId,
        ProductId,
        PriceThreshold,
        CreatedAt
    )
    {
    }
};

public record PriceAlertId(
    string Value
);