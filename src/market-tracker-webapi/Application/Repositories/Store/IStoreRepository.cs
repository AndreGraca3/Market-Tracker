using market_tracker_webapi.Application.Models;

namespace market_tracker_webapi.Application.Repositories.Store
{
    public interface IStoreRepository
    {
        Task<StoreData?> GetStoreByIdAsync(int id);

        Task<int?> AddStoreAsync(StoreData storeData);

        Task<StoreData?> UpdateStoreAsync(StoreData storeData);
        
        Task<StoreData?> DeleteStoreAsync(int id);

        Task<IEnumerable<StoreData>> GetStoresFromCompany(int id);
        
        Task<IEnumerable<StoreData>> GetStoresFromCityByName(string name);
    }
}
