using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Operations.Store
{
    public interface IStoreRepository
    {
        Task<IEnumerable<Domain.Store>> GetStoresAsync(int? companyId = null, int? cityId = null, string? storeName = null);

        Task<Domain.Store?> GetStoreByIdAsync(int id);

        public Task<Domain.Store?> GetStoreByNameAsync(string name);
        
        Task<int> AddStoreAsync(string name, string address, int? cityId, int companyId);

        Task<Domain.Store?> UpdateStoreAsync(int id, string address, int cityId, int companyId);

        Task<Domain.Store?> DeleteStoreAsync(int id);
    }
}
