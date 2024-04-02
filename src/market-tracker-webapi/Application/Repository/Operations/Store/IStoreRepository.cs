using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Operations.Store
{
    public interface IStoreRepository
    {
        Task<IEnumerable<Domain.Store>> GetStoresAsync();

        Task<Domain.Store?> GetStoreByIdAsync(int id);

        Task<Domain.Store?> GetStoreByNameAsync(string name);

        Task<Domain.Store?> GetStoreByAddressAsync(string address);

        Task<IEnumerable<Domain.Store>> GetStoresFromCompanyAsync(int id);

        Task<IEnumerable<Domain.Store>> GetStoresByCityNameAsync(string cityName);

        Task<int> AddStoreAsync(string name, string address, int cityId, int companyId);

        Task<Domain.Store?> UpdateStoreAsync(int id, string address, int cityId, int companyId);

        Task<Domain.Store?> DeleteStoreAsync(int id);
    }
}
