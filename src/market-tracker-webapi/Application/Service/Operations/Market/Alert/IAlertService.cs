using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales.Pricing;

namespace market_tracker_webapi.Application.Service.Operations.Market.Alert;

public interface IAlertService
{
    Task<IEnumerable<PriceAlert>> GetPriceAlertsByClientIdAsync(Guid clientId, string? productId, int? storeId);

    Task<PriceAlertId> AddPriceAlertAsync(
        Guid clientId,
        string productId,
        int storeId,
        int priceThreshold
    );

    Task<PriceAlert> RemovePriceAlertAsync(Guid clientId, string alertId);
}