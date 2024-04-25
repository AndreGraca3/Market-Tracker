using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.Store;

public class StoreRepository(MarketTrackerDataContext marketTrackerDataContext) : IStoreRepository
{
    public async Task<IEnumerable<Domain.Store>> GetStoresAsync(int? companyId, int? cityId, string? storeName)
    {
        var storeEntities = await marketTrackerDataContext.Store
            .Where(s => companyId == null || s.CompanyId == companyId)
            .Where(s => cityId == null || s.CityId == cityId)
            .Where(s => storeName == null || EF.Functions.ILike(s.Name, $"%{storeName}%"))
            .ToListAsync();

        return storeEntities.Select(s => s.ToStore());
    }

    public async Task<Domain.Store?> GetStoreByIdAsync(int id)
    {
        var storeEntity = await marketTrackerDataContext.Store.FindAsync(id);
        return storeEntity?.ToStore();
    }

    public async Task<Domain.Store?> GetStoreByNameAsync(string name)
    {
        var storeEntity = await marketTrackerDataContext.Store
            .Where(s => s.Name == name)
            .FirstOrDefaultAsync();

        return storeEntity?.ToStore();
    }
    
    public async Task<int> AddStoreAsync(string name, string address, int? cityId, int companyId)
    {
        var newStore = new StoreEntity
        {
            Name = name,
            Address = address,
            CityId = cityId,
            CompanyId = companyId
        };

        marketTrackerDataContext.Store.Add(newStore);
        await marketTrackerDataContext.SaveChangesAsync();

        return newStore.Id;
    }

    public async Task<Domain.Store?> UpdateStoreAsync(
        int id,
        string address,
        int cityId,
        int companyId
    )
    {
        var storeEntity = await marketTrackerDataContext.Store.FindAsync(id);

        if (storeEntity == null)
        {
            return null;
        }

        storeEntity.Address = address;
        storeEntity.CityId = cityId;
        storeEntity.CompanyId = companyId;

        await marketTrackerDataContext.SaveChangesAsync();
        return storeEntity.ToStore();
    }

    public async Task<Domain.Store?> DeleteStoreAsync(int id)
    {
        var storeEntity = await marketTrackerDataContext.Store.FindAsync(id);

        if (storeEntity == null)
        {
            return null;
        }

        marketTrackerDataContext.Store.Remove(storeEntity);
        await marketTrackerDataContext.SaveChangesAsync();

        return storeEntity.ToStore();
    }
}
