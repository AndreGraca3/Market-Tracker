namespace market_tracker_webapi.Application.Repository.Operations.Market.Company;

public interface ICompanyRepository
{
    Task<IEnumerable<Domain.Company>> GetCompaniesAsync();
    Task<Domain.Company?> GetCompanyByIdAsync(int id);

    Task<Domain.Company?> GetCompanyByNameAsync(string name);

    Task<int> AddCompanyAsync(string name);

    Task<Domain.Company?> UpdateCompanyAsync(int id, string name);

    Task<Domain.Company?> DeleteCompanyAsync(int id);
}
