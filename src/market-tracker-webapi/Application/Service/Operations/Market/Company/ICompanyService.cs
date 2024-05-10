using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Company;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.Company;

public interface ICompanyService
{
    Task<Either<IServiceError, CollectionOutputModel<Domain.Models.Market.Retail.Shop.Company>>> GetCompaniesAsync();

    Task<Either<CompanyFetchingError, Domain.Models.Market.Retail.Shop.Company>> GetCompanyByIdAsync(int id);

    Task<Either<CompanyFetchingError, Domain.Models.Market.Retail.Shop.Company>> GetCompanyByNameAsync(string companyName);

    Task<Either<ICompanyError, IntIdOutputModel>> AddCompanyAsync(string companyName);

    Task<Either<CompanyFetchingError, IntIdOutputModel>> DeleteCompanyAsync(int id);

    Task<Either<ICompanyError, Domain.Models.Market.Retail.Shop.Company>> UpdateCompanyAsync(int id, string companyName);
}
