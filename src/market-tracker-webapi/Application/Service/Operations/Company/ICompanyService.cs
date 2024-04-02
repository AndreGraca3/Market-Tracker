using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Service.Errors.Company;
using market_tracker_webapi.Application.Services.Errors.Company;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Company;

public interface ICompanyService
{
    Task<CollectionOutputModel> GetCompaniesAsync();

    Task<Either<CompanyFetchingError, Domain.Company>> GetCompanyByIdAsync(int id);

    Task<Either<CompanyFetchingError, Domain.Company>> GetCompanyByNameAsync(string companyName);

    Task<Either<ICompanyError, IdOutputModel>> AddCompanyAsync(string companyName);

    Task<Either<CompanyFetchingError, IdOutputModel>> DeleteCompanyAsync(int id);

    Task<Either<ICompanyError, Domain.Company>> UpdateCompanyAsync(int id, string companyName);
}
