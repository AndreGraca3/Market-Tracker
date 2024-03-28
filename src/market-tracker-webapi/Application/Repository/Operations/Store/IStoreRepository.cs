using market_tracker_webapi.Application.Models;

namespace market_tracker_webapi.Application.Repositories.Store
{
    public interface IStoreRepository
    {
        Task<Models.StoreData> GetStoreAsync(int id);

        Task<int> AddStoreAsync(string address, string city, DateTime openTime, DateTime closeTime, int companyId);

        Task<Models.StoreData> UpdateStoreAsync(int id, DateTime? openTime, DateTime? closeTime);
        
        Task DeleteStoreAsync(int id);

        Task<List<Models.StoreData>> GetStoresFromCompany(int id);

        Task<CompanyData> GetCompanyAsync(int id);

        Task<int> AddCompanyAsync(string companyName);

        Task<int> DeleteCompanyAsync(int id);
    }
}
