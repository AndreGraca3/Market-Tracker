using market_tracker_webapi.Application.Domain.Models.Market.Retail.Shop;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Company;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.Company;

using Company = Domain.Models.Market.Retail.Shop.Company;

public interface ICompanyService
{
    Task<Either<IServiceError, IEnumerable<Company>>> GetCompaniesAsync();

    Task<Either<CompanyFetchingError, Company>> GetCompanyByIdAsync(int id);

    Task<Either<CompanyFetchingError, Company>> GetCompanyByNameAsync(string companyName);

    Task<Either<ICompanyError, CompanyId>> AddCompanyAsync(string companyName);

    Task<Either<ICompanyError, Company>> UpdateCompanyAsync(int id, string companyName);

    Task<Either<CompanyFetchingError, CompanyId>> DeleteCompanyAsync(int id);
}