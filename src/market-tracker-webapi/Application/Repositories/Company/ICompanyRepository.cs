using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repositories.Company;

public interface ICompanyRepository
{
    Task<IEnumerable<CompanyDomain>> GetCompaniesAsync();
    Task<CompanyDomain?> GetCompanyByIdAsync(int id);
    
    Task<CompanyDomain?> GetCompanyByNameAsync(string name);
    
    Task<int> AddCompanyAsync(string name);
    
    Task<CompanyDomain?> UpdateCompanyAsync(int id, string name);
    
    Task<CompanyDomain?> DeleteCompanyAsync(int id);
}