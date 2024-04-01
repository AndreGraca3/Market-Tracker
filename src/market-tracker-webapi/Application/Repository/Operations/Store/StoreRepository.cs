using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.Store;

public class StoreRepository(MarketTrackerDataContext marketTrackerDataContext) : IStoreRepository
{
    
    public async Task<IEnumerable<StoreDomain>> GetStoresAsync()
    {
        var storeEntities = await marketTrackerDataContext.Store.ToListAsync();
        return storeEntities.Select(MapStoreEntity);
    }
    
    public async Task<StoreDomain?> GetStoreByIdAsync(int id)
    {
        var storeEntity = await marketTrackerDataContext.Store.FindAsync(id);
        return storeEntity != null ? MapStoreEntity(storeEntity) : null;
    }
    
    public async Task<StoreDomain?> GetStoreByNameAsync(string name)
    {
        var storeEntity = await marketTrackerDataContext.Store
            .FirstOrDefaultAsync(s => s.Name == name);
        
        return storeEntity != null ? MapStoreEntity(storeEntity) : null;
    }
    
    public async Task<StoreDomain?> GetStoreByAddressAsync(string address)
    {
        var storeEntity = await marketTrackerDataContext.Store
            .FirstOrDefaultAsync(s => s.Address == address);
        
        return storeEntity != null ? MapStoreEntity(storeEntity) : null;
    }
    
    public async Task<IEnumerable<StoreDomain>> GetStoresFromCompanyAsync(int id)
    {
        var stores = await marketTrackerDataContext.Store
            .Where(s => s.CompanyId == id)
            .ToListAsync();
        
        return stores.Select(MapStoreEntity);
    }

    public async Task<IEnumerable<StoreDomain>> GetStoresByCityNameAsync(string name)
    {
        var city = await marketTrackerDataContext.City
            .FirstOrDefaultAsync(c => c.Name == name);
        
        if (city == null)
        {
            return new List<StoreDomain>();
        }
        
        var storeEntities = await marketTrackerDataContext.Store
            .Where(s => s.CityId == city.Id)
            .ToListAsync();
        
        return storeEntities.Select(MapStoreEntity);
    }

    public async Task<int> AddStoreAsync(string name, string address, int cityId, int companyId)
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

    public async Task<StoreDomain?> UpdateStoreAsync(int id, string address, int cityId, int companyId)
    {
        var currentStore = await marketTrackerDataContext.Store.FindAsync(id);
        
        if(currentStore == null)
        {
            return null;
        }
        
        currentStore.Address = address;
        currentStore.CityId = cityId;
        currentStore.CompanyId = companyId;
        
        await marketTrackerDataContext.SaveChangesAsync();
        return MapStoreEntity(currentStore);
    }

    public async Task<StoreDomain?> DeleteStoreAsync(int id)
    {
        var currentStore = await marketTrackerDataContext.Store.FindAsync(id);
        
        if (currentStore == null)
        {
            return null;
        }
        
        marketTrackerDataContext.Store.Remove(currentStore);
        await marketTrackerDataContext.SaveChangesAsync();
        
        return MapStoreEntity(currentStore);
    }

    private static StoreDomain MapStoreEntity(StoreEntity storeEntity)
    {
        return new StoreDomain
        {
            Id = storeEntity.Id,
            Name = storeEntity.Name,
            Address = storeEntity.Address,
            CityId = storeEntity.CityId,
            CompanyId = storeEntity.CompanyId
        };
    }
}
