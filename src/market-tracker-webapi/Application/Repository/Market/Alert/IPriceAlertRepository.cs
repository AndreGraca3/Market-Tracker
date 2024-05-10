using market_tracker_webapi.Application.Domain.Models.Market.Retail.Sales.Pricing;

namespace market_tracker_webapi.Application.Repository.Market.Alert;

public interface IPriceAlertRepository
{
    Task<IEnumerable<PriceAlert>> GetPriceAlertsAsync(Guid? clientId = null, string? productId = null,
        int? storeId = null, int? minPriceThreshold = null);

    Task<PriceAlert?> GetPriceAlertAsync(Guid clientId, string productId, int storeId);

    Task<PriceAlert> AddPriceAlertAsync(
        Guid clientId,
        string productId,
        int storeId,
        int priceThreshold
    );

    Task<PriceAlert?> RemovePriceAlertAsync(string alertId);
}