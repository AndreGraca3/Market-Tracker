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
            .Join(dataContext.Product, alert => alert.ProductId, product => product.Id,
                (alert, product) => new { alert, product })
            .Join(dataContext.Store, g => g.alert.StoreId, store => store.Id,
                (g, store) => new { g.alert, g.product, store })
            .Where(g => clientId == null || g.alert.ClientId == clientId)
            .Where(g => productId == null || g.alert.ProductId == productId)
            .Where(g => storeId == null || g.alert.StoreId == storeId)
            .Where(g => minPriceThreshold == null || g.alert.PriceThreshold >= minPriceThreshold)
            .Select(g => new PriceAlert(
                new PriceAlertId(g.alert.Id),
                g.alert.ClientId,
                new PriceAlertProduct(g.product.Id, g.product.Name, g.product.ImageUrl),
                new PriceAlertStore(g.alert.StoreId, g.store.Name),
                g.alert.PriceThreshold,
                g.alert.CreatedAt
            ))
            .ToListAsync();
    }

    public async Task<PriceAlert?> GetPriceAlertAsync(Guid clientId, string productId, int storeId)
    {
        var priceAlertEntity = await dataContext.PriceAlert
            .Join(dataContext.Product, alert => alert.ProductId, product => product.Id,
                (alert, product) => new { alert, product })
            .Join(dataContext.Store, g => g.alert.StoreId, store => store.Id,
                (g, store) => new { g.alert, g.product, store })
            .FirstOrDefaultAsync(g =>
                g.alert.ClientId == clientId && g.alert.ProductId == productId && g.alert.StoreId == storeId);
        return priceAlertEntity?.alert.ToPriceAlert(priceAlertEntity.product.Name, priceAlertEntity.product.ImageUrl,
            priceAlertEntity.store.Name);
    }

    public async Task<PriceAlertId> AddPriceAlertAsync(
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
            CreatedAt = DateTime.UtcNow
        };
        await dataContext.PriceAlert.AddAsync(priceAlertEntity);
        await dataContext.SaveChangesAsync();
        return new PriceAlertId(priceAlertEntity.Id);
    }

    public async Task<PriceAlert?> RemovePriceAlertByIdAsync(string alertId)
    {
        var priceAlertEntity = await dataContext.PriceAlert
            .Join(dataContext.Product, alert => alert.ProductId, product => product.Id,
                (alert, product) => new { alert, product })
            .Join(dataContext.Store, g => g.alert.StoreId, store => store.Id,
                (g, store) => new { g.alert, g.product, store })
            .FirstOrDefaultAsync(g => g.alert.Id == alertId);

        if (priceAlertEntity is null)
        {
            return null;
        }

        dataContext.PriceAlert.Remove(priceAlertEntity.alert);
        await dataContext.SaveChangesAsync();
        return priceAlertEntity.alert.ToPriceAlert(priceAlertEntity.product.Name, 
            priceAlertEntity.product.ImageUrl, priceAlertEntity.store.Name);
    }
}