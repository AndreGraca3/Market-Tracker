using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.Alert;

public class PriceAlertRepository(MarketTrackerDataContext dataContext) : IPriceAlertRepository
{
    public async Task<IEnumerable<PriceAlert>> GetPriceAlertsByClientIdAsync(Guid clientId, string? productId)
    {
        return await dataContext.PriceAlert
            .Where(alert => productId == null || alert.ProductId == productId)
            .Where(alert => alert.ClientId == clientId)
            .Select(alert => alert.ToPriceAlert())
            .ToListAsync();
    }
    
    public async Task<PriceAlert?> GetPriceAlertByClientIdAndProductIdAsync(Guid clientId, string productId)
    {
        var priceAlertEntity = await dataContext.PriceAlert
            .FirstOrDefaultAsync(alert => alert.ClientId == clientId && alert.ProductId == productId);
        return priceAlertEntity?.ToPriceAlert();
    }

    public async Task<PriceAlert> AddPriceAlertAsync(
        Guid clientId,
        string productId,
        int priceThreshold
    )
    {
        var priceAlertEntity = new PriceAlertEntity
        {
            ProductId = productId,
            ClientId = clientId,
            PriceThreshold = priceThreshold
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