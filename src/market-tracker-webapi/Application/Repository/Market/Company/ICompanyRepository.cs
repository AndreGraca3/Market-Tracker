namespace market_tracker_webapi.Application.Repository.Operations.Market.Company;

public interface ICompanyRepository
{
    Task<IEnumerable<Domain.Models.Market.Store.Company>> GetCompaniesAsync();
    Task<Domain.Models.Market.Store.Company?> GetCompanyByIdAsync(int id);

    Task<Domain.Models.Market.Store.Company?> GetCompanyByNameAsync(string name);

    Task<int> AddCompanyAsync(string name);

    Task<Domain.Models.Market.Store.Company?> UpdateCompanyAsync(int id, string name);

    Task<Domain.Models.Market.Store.Company?> DeleteCompanyAsync(int id);
}
