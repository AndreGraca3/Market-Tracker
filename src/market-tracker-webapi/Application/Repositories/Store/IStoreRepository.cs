using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repositories.Store
{
    public interface IStoreRepository
    {
        Task<StoreDomain?> GetStoreByIdAsync(int id);

        Task<int> AddStoreAsync(string address, int cityId, int companyId);

        Task<StoreDomain?> UpdateStoreAsync(int id, string address, int cityId, int companyId);
        
        Task<StoreDomain?> DeleteStoreAsync(int id);

        Task<IEnumerable<StoreDomain>> GetStoresFromCompany(int id);
        
        Task<IEnumerable<StoreDomain>> GetStoresFromCityByName(string name);
    }
}
