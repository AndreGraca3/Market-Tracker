namespace market_tracker_webapi.Application.Repository.Market.Company;

public interface ICompanyRepository
{
    Task<IEnumerable<Domain.Models.Market.Retail.Shop.Company>> GetCompaniesAsync();
    Task<Domain.Models.Market.Retail.Shop.Company?> GetCompanyByIdAsync(int id);

    Task<Domain.Models.Market.Retail.Shop.Company?> GetCompanyByNameAsync(string name);

    Task<int> AddCompanyAsync(string name);

    Task<Domain.Models.Market.Retail.Shop.Company?> UpdateCompanyAsync(int id, string name);

    Task<Domain.Models.Market.Retail.Shop.Company?> DeleteCompanyAsync(int id);
}
