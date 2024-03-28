using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Service.Operations.Store
{
    public interface IStoreService
    {
        Task<StoreDomain> GetStoreByIdAsync(int id);
    
        Task<int> AddStoreAsync(StoreDomain storeDomain);
    
        Task<StoreDomain> UpdateStoreAsync(StoreDomain storeDomain);
        
        Task<StoreDomain> DeleteStoreAsync(int id);
    
        Task<IEnumerable<StoreDomain>> GetStoresFromCompany(int id);
    
        Task<CompanyDomain> GetCompanyAsync(int id);
    
        Task<int> AddCompanyAsync(string companyName);
    
        Task<int> DeleteCompanyAsync(int id);
    }
}