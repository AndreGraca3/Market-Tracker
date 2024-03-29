using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repositories.Store
{
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
        
        public async Task<StoreDomain?> GetStoreByAddressAsync(string address)
        {
            var storeEntity = await marketTrackerDataContext.Store
                .FirstOrDefaultAsync(s => s.Address == address);
            
            return storeEntity != null ? MapStoreEntity(storeEntity) : null;
        }

        public async Task<int> AddStoreAsync(string address, int cityId, int companyId)
        {
            var newStore = new StoreEntity
            {
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
            
            //var company = await marketTrackerDataContext.Company.FindAsync(companyId);
            // var city = cityId;
            //
            // if(currentStore == null || company == null)
            // {
            //     return null;
            // }
            //  
            // if (city != null)
            // {
            //     var currentCity = await marketTrackerDataContext.City.FindAsync(cityId);
            //
            //     if (currentCity == null)
            //     {
            //         return null;
            //     }
            //
            //     city = currentCity.Id;
            // }
            
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

        public async Task<IEnumerable<StoreDomain>> GetStoresFromCompany(int id)
        {
            var stores = await marketTrackerDataContext.Store
                .Where(s => s.CompanyId == id)
                .ToListAsync();
            
            return stores.Select(MapStoreEntity);
        }

        public async Task<IEnumerable<StoreDomain>> GetStoresFromCityByName(string name)
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

        private static StoreDomain MapStoreEntity(StoreEntity storeEntity)
        {
            return new StoreDomain
            {
                Id = storeEntity.Id,
                Address = storeEntity.Address,
                CityId = storeEntity.CityId,
                CompanyId = storeEntity.CompanyId
            };
        }
    }
}