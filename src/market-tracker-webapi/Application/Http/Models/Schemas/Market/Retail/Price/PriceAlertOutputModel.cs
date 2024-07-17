using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales.Pricing;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Price;

public record PriceAlertOutputModel(
    string Id,
    PriceAlertProduct Product,
    PriceAlertStore Store,
    int PriceThreshold,
    DateTime CreatedAt
);

public static class PriceAlertOutputModelMapper
{
    public static PriceAlertOutputModel ToOutputModel(this PriceAlert priceAlert)
    {
        return new PriceAlertOutputModel(
            priceAlert.Id.Value,
            priceAlert.Product,
            priceAlert.Store,
            priceAlert.PriceThreshold,
            priceAlert.CreatedAt
        );
    }
}