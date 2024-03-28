using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Services.Errors.Company;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Services.Operations.Company;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDomain>> GetCompaniesAsync();
    
    Task<Either<CompanyFetchingError, CompanyDomain>> GetCompanyByIdAsync(int id);
    
    Task<Either<CompanyFetchingError, CompanyDomain>> GetCompanyByNameAsync(string companyName);
    
    Task<Either<ICompanyError, IdOutputModel>> AddCompanyAsync(string companyName);
    
    Task<Either<CompanyFetchingError, IdOutputModel>> DeleteCompanyAsync(int id);
    
    Task<Either<ICompanyError, CompanyDomain>> UpdateCompanyAsync(int id, string companyName);
}