using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repositories.Store
{
    public class StoreRepository(MarketTrackerDataContext marketTrackerDataContext) : IStoreRepository
    {
        public async Task<StoreData?> GetStoreByIdAsync(int id)
        {
            var storeEntity = await marketTrackerDataContext.Store.FindAsync(id);
            return storeEntity != null ? MapStoreEntity(storeEntity) : null;
        }

        public async Task<int?> AddStoreAsync(StoreData storeData)
        {
            var company = await marketTrackerDataContext.Company.FindAsync(storeData.CompanyId);

            if (company == null)
            {
                return null;
            }
            
            var newStore = new StoreEntity
            {
                Address = storeData.Address,
                CityId = storeData.CityId,
                CompanyId = storeData.CompanyId
            };
            
            marketTrackerDataContext.Store.Add(newStore);
            await marketTrackerDataContext.SaveChangesAsync();

            return newStore.Id;
        }

        public async Task<StoreData?> UpdateStoreAsync(StoreData storeData)
        {
            var currentStore = await marketTrackerDataContext.Store.FindAsync(storeData.Id);
            var company = await marketTrackerDataContext.Company.FindAsync(storeData.CompanyId);
            var city = storeData.CityId;

            if(currentStore == null || company == null)
            {
                return null;
            }
            
            if (city != null)
            {
                var currentCity = await marketTrackerDataContext.City.FindAsync(storeData.CityId);
                
                if (currentCity == null)
                {
                    return null;
                }
                city = currentCity.Id;
            }
            
            currentStore.Address = storeData.Address;
            currentStore.CityId = city;
            currentStore.CompanyId = storeData.CompanyId;
            
            //await marketTrackerDataContext.SaveChangesAsync();
            return MapStoreEntity(currentStore);
        }

        public async Task<StoreData?> DeleteStoreAsync(int id)
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

        public async Task<IEnumerable<StoreData>> GetStoresFromCompany(int id)
        {
            var stores = await marketTrackerDataContext.Store
                .Where(s => s.CompanyId == id)
                .ToListAsync();
            
            return stores.Select(MapStoreEntity);
        }

        public async Task<IEnumerable<StoreData>> GetStoresFromCityByName(string name)
        {
            var city = await marketTrackerDataContext.City
                .FirstOrDefaultAsync(c => c.Name == name);
            
            if (city == null)
            {
                return new List<StoreData>();;
            }
            
            var storeEntities = await marketTrackerDataContext.Store
                .Where(s => s.CityId == city.Id)
                .ToListAsync();
            
            return storeEntities.Select(MapStoreEntity);
        }

        private static StoreData MapStoreEntity(StoreEntity storeEntity)
        {
            return new StoreData
            {
                Id = storeEntity.Id,
                Address = storeEntity.Address,
                CityId = storeEntity.CityId,
                CompanyId = storeEntity.CompanyId
            };
        }
    }
}