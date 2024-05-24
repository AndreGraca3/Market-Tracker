using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Market.Company;

using Company = Domain.Schemas.Market.Retail.Shop.Company;

public class CompanyRepository(MarketTrackerDataContext marketTrackerDataContext)
    : ICompanyRepository
{
    public async Task<IEnumerable<Company>> GetCompaniesAsync()
    {
        var companies = await marketTrackerDataContext.Company.ToListAsync();
        return companies.Select(c => c.ToCompany());
    }

    public async Task<Company?> GetCompanyByIdAsync(int id)
    {
        var companyEntity = await marketTrackerDataContext.Company.FindAsync(id);
        return companyEntity?.ToCompany();
    }

    public async Task<Company?> GetCompanyByNameAsync(string name)
    {
        var companyEntity = await marketTrackerDataContext.Company.FirstOrDefaultAsync(c =>
            c.Name == name
        );
        return companyEntity?.ToCompany();
    }

    public async Task<CompanyId> AddCompanyAsync(string name, string logoUrl)
    {
        var newCompany = new CompanyEntity { Name = name, LogoUrl = logoUrl, CreatedAt = DateTime.Now };

        marketTrackerDataContext.Company.Add(newCompany);
        await marketTrackerDataContext.SaveChangesAsync();

        return new CompanyId(newCompany.Id);
    }

    public async Task<Company?> UpdateCompanyAsync(int id, string name)
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

    public async Task<Company?> DeleteCompanyAsync(int id)
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