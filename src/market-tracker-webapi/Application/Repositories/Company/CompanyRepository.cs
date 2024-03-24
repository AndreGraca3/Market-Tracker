using market_tracker_webapi.Application.Models.Company;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;

namespace market_tracker_webapi.Application.Repositories.Company;

public class CompanyRepository(MarketTrackerDataContext marketTrackerDataContext) : ICompanyRepository
{
    public async Task<CompanyData?> GetCompanyByIdAsync(int id)
    {
        var companyEntity = await marketTrackerDataContext.Company.FindAsync(id);
        return companyEntity != null ? MapCompanyEntity(companyEntity) : null;
    }

    public async Task<int?> AddCompanyAsync(CompanyAddInputData companyData)
    {
        var newCompany = new CompanyEntity
        {
            Name = companyData.Name
        };
        
        marketTrackerDataContext.Company.Add(newCompany);
        await marketTrackerDataContext.SaveChangesAsync();
        
        return newCompany.Id;
    }

    public async Task<CompanyData?> UpdateCompanyAsync(CompanyUpdateInputData companyData)
    {
        var currentCompany = await marketTrackerDataContext.Company.FindAsync(companyData.Id);
        
        if (currentCompany == null)
        {
            return null;
        }
        
        currentCompany.Name = companyData.Name;
        
        //await marketTrackerDataContext.SaveChangesAsync();
        return MapCompanyEntity(currentCompany);
    }

    public async Task<CompanyData?> DeleteCompanyAsync(int id)
    {
        var companyEntity = await marketTrackerDataContext.Company.FindAsync(id);
        
        if (companyEntity == null)
        {
            return null;
        }
        
        marketTrackerDataContext.Company.Remove(companyEntity);
        //await marketTrackerDataContext.SaveChangesAsync();
        
        return MapCompanyEntity(companyEntity);
    }
    
    private static CompanyData MapCompanyEntity(CompanyEntity companyEntity)
    {
        return new CompanyData
        {
            Id = companyEntity.Id,
            Name = companyEntity.Name,
            CreatedAt = companyEntity.CreatedAt
        };
    }
}