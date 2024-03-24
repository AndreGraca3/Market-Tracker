using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Models.Company;

namespace market_tracker_webapi.Application.Services.Store
{
    public interface IStoreService
    {
        Task<StoreData> GetStoreByIdAsync(int id);
    
        Task<int> AddStoreAsync(StoreData storeData);
    
        Task<StoreData> UpdateStoreAsync(StoreData storeData);
        
        Task<StoreData> DeleteStoreAsync(int id);
    
        Task<IEnumerable<StoreData>> GetStoresFromCompany(int id);
    
        Task<CompanyData> GetCompanyAsync(int id);
    
        Task<int> AddCompanyAsync(string companyName);
    
        Task<int> DeleteCompanyAsync(int id);
    }
}