using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales.Pricing;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Market.Alert;

public class PriceAlertRepository(MarketTrackerDataContext dataContext) : IPriceAlertRepository
{
    public async Task<IEnumerable<PriceAlert>> GetPriceAlertsAsync(Guid? clientId, string? productId,
        int? storeId, int? minPriceThreshold)
    {
        return await dataContext.PriceAlert
            .Where(alert => clientId == null || alert.ClientId == clientId)
            .Where(alert => productId == null || alert.ProductId == productId)
            .Where(alert => storeId == null || alert.StoreId == storeId)
            .Where(alert => minPriceThreshold == null || alert.PriceThreshold >= minPriceThreshold)
            .Select(alert => alert.ToPriceAlert())
            .ToListAsync();
    }

    public async Task<PriceAlert?> GetPriceAlertAsync(Guid clientId, string productId, int storeId)
    {
        var priceAlertEntity = await dataContext.PriceAlert
            .FirstOrDefaultAsync(alert => alert.ClientId == clientId && alert.ProductId == productId);
        return priceAlertEntity?.ToPriceAlert();
    }

    public async Task<PriceAlert> AddPriceAlertAsync(
        Guid clientId,
        string productId,
        int storeId,
        int priceThreshold
    )
    {
        var priceAlertEntity = new PriceAlertEntity
        {
            ClientId = clientId,
            ProductId = productId,
            StoreId = storeId,
            PriceThreshold = priceThreshold,
            CreatedAt = DateTime.Now
        };
        await dataContext.PriceAlert.AddAsync(priceAlertEntity);
        await dataContext.SaveChangesAsync();
        return priceAlertEntity.ToPriceAlert();
    }

    public async Task<PriceAlert?> RemovePriceAlertAsync(string alertId)
    {
        var priceAlertEntity = await dataContext.PriceAlert.FindAsync(alertId);
        if (priceAlertEntity is null)
        {
            return null;
        }

        dataContext.PriceAlert.Remove(priceAlertEntity);
        await dataContext.SaveChangesAsync();
        return priceAlertEntity.ToPriceAlert();
    }
}