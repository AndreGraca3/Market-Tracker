using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.Market.Company;

public class CompanyRepository(MarketTrackerDataContext marketTrackerDataContext)
    : ICompanyRepository
{
    public async Task<IEnumerable<Domain.Company>> GetCompaniesAsync()
    {
        var companies = await marketTrackerDataContext.Company.ToListAsync();
        return companies.Select(c => c.ToCompany());
    }

    public async Task<Domain.Company?> GetCompanyByIdAsync(int id)
    {
        var companyEntity = await marketTrackerDataContext.Company.FindAsync(id);
        return companyEntity?.ToCompany();
    }

    public async Task<Domain.Company?> GetCompanyByNameAsync(string name)
    {
        var companyEntity = await marketTrackerDataContext.Company.FirstOrDefaultAsync(c =>
            c.Name == name
        );
        return companyEntity?.ToCompany();
    }

    public async Task<int> AddCompanyAsync(string name)
    {
        var newCompany = new CompanyEntity { Name = name };

        marketTrackerDataContext.Company.Add(newCompany);
        await marketTrackerDataContext.SaveChangesAsync();

        return newCompany.Id;
    }

    public async Task<Domain.Company?> UpdateCompanyAsync(int id, string name)
    {
        var currentCompany = await marketTrackerDataContext.Company.FindAsync(id);

        if (currentCompany == null)
        {
            return null;
        }

        currentCompany.Name = name;

        await marketTrackerDataContext.SaveChangesAsync();
        return currentCompany.ToCompany();
    }

    public async Task<Domain.Company?> DeleteCompanyAsync(int id)
    {
        var companyEntity = await marketTrackerDataContext.Company.FindAsync(id);

        if (companyEntity == null)
        {
            return null;
        }

        marketTrackerDataContext.Company.Remove(companyEntity);
        await marketTrackerDataContext.SaveChangesAsync();

        return companyEntity.ToCompany();
    }
}
