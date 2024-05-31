using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;

namespace market_tracker_webapi.Application.Service.Operations.Market.Company;

using Company = Domain.Schemas.Market.Retail.Shop.Company;

public interface ICompanyService
{
    Task<IEnumerable<Company>> GetCompaniesAsync();

    Task<Company> GetCompanyByIdAsync(int id);

    Task<Company> GetCompanyByNameAsync(string companyName);

    Task<CompanyId> AddCompanyAsync(string companyName, string companyLogoUrl);

    Task<Company> UpdateCompanyAsync(int id, string companyName);

    Task<CompanyId> DeleteCompanyAsync(int id);
}