using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Operations.Alert;

public interface IPriceAlertRepository
{
    Task<IEnumerable<PriceAlert>> GetPriceAlertsByClientIdAsync(Guid clientId, string? productId = null);

    Task<PriceAlert?> GetPriceAlertByClientIdAndProductIdAsync(Guid clientId, string productId);
    
    Task<PriceAlert> AddPriceAlertAsync(
        Guid clientId,
        string productId,
        int priceThreshold
    );

    Task<PriceAlert?> RemovePriceAlertAsync(string alertId);
}