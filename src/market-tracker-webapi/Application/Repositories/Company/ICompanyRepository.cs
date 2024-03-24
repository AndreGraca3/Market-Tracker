using market_tracker_webapi.Application.Models.Company;

namespace market_tracker_webapi.Application.Repositories.Company;

public interface ICompanyRepository
{
    Task<CompanyData?> GetCompanyByIdAsync(int id);
    
    Task<int?> AddCompanyAsync(CompanyAddInputData companyData);
    
    Task<CompanyData?> UpdateCompanyAsync(CompanyUpdateInputData companyData);
    
    Task<CompanyData?> DeleteCompanyAsync(int id);
}