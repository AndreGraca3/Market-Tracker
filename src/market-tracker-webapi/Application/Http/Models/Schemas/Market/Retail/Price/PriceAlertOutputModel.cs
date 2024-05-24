using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales.Pricing;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Price;

public record PriceAlertOutputModel(
    string Id,
    string ProductId,
    int StoreId,
    decimal PriceThreshold,
    DateTime CreatedAt
);

public static class PriceAlertOutputModelMapper
{
    public static PriceAlertOutputModel ToOutputModel(this PriceAlert priceAlert)
    {
        return new PriceAlertOutputModel(
            priceAlert.Id.Value,
            priceAlert.ProductId,
            priceAlert.StoreId,
            priceAlert.PriceThreshold,
            priceAlert.CreatedAt
        );
    }
}