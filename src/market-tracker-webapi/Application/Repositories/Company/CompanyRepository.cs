using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repositories.Company;

public class CompanyRepository(MarketTrackerDataContext marketTrackerDataContext) : ICompanyRepository
{
    public async Task<IEnumerable<CompanyDomain>> GetCompaniesAsync()
    {
        var companies = await marketTrackerDataContext.Company.ToListAsync();
        return companies.Select(MapCompanyEntity);
    }

    public async Task<CompanyDomain?> GetCompanyByIdAsync(int id)
    {
        var companyEntity = await marketTrackerDataContext.Company.FindAsync(id);
        return companyEntity != null ? MapCompanyEntity(companyEntity) : null;
    }

    public async Task<CompanyDomain?> GetCompanyByNameAsync(string name)
    {
        var companyEntity = await marketTrackerDataContext.Company.FirstOrDefaultAsync(c => c.Name == name);
        return companyEntity != null ? MapCompanyEntity(companyEntity) : null;
    }
    
    public async Task<int> AddCompanyAsync(string name)
    {
        var newCompany = new CompanyEntity
        {
            Name = name
        };
        
        marketTrackerDataContext.Company.Add(newCompany);
        await marketTrackerDataContext.SaveChangesAsync();
        
        return newCompany.Id;
    }

    public async Task<CompanyDomain?> UpdateCompanyAsync(int id, string name)
    {
        var currentCompany = await marketTrackerDataContext.Company.FindAsync(id);
        
        if (currentCompany == null)
        {
            return null;
        }
        
        currentCompany.Name = name;
        
        await marketTrackerDataContext.SaveChangesAsync();
        return MapCompanyEntity(currentCompany);
    }

    public async Task<CompanyDomain?> DeleteCompanyAsync(int id)
    {
        var companyEntity = await marketTrackerDataContext.Company.FindAsync(id);
        
        if (companyEntity == null)
        {
            return null;
        }
        
        marketTrackerDataContext.Company.Remove(companyEntity);
        await marketTrackerDataContext.SaveChangesAsync();
        
        return MapCompanyEntity(companyEntity);
    }
    
    private static CompanyDomain MapCompanyEntity(CompanyEntity companyEntity)
    {
        return new CompanyDomain
        {
            Id = companyEntity.Id,
            Name = companyEntity.Name,
            CreatedAt = companyEntity.CreatedAt
        };
    }
}