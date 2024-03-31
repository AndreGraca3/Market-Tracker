using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repositories.Store
{
    public interface IStoreRepository
    {
        Task<IEnumerable<StoreDomain>> GetStoresAsync();
        
        Task<StoreDomain?> GetStoreByIdAsync(int id);
        
        Task<StoreDomain?> GetStoreByAddressAsync(string address);
        
        Task<IEnumerable<StoreDomain>> GetStoresFromCompanyAsync(int id);
        
        Task<IEnumerable<StoreDomain>> GetStoresByCityNameAsync(string cityName);

        Task<int> AddStoreAsync(string name, string address, int cityId, int companyId);

        Task<StoreDomain?> UpdateStoreAsync(int id, string address, int cityId, int companyId);
        
        Task<StoreDomain?> DeleteStoreAsync(int id);
    }
}
