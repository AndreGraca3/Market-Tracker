using market_tracker_webapi.Application.Models;

namespace market_tracker_webapi.Application.Repositories.Store
{
    public interface IStoreRepository
    {
        Task<StoreData?> GetStoreByIdAsync(int id);

        Task<int> AddStoreAsync(StoreData storeData);

        Task<StoreData?> UpdateStoreAsync(int id, DateTime? openTime, DateTime? closeTime);
        
        Task DeleteStoreAsync(int id);

        Task<IEnumerable<StoreData>> GetStoresFromCompany(int id);

        Task<CompanyData> GetCompanyAsync(int id);

        Task<int> AddCompanyAsync(string companyName);

        Task<int> DeleteCompanyAsync(int id);
    }
}
