using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repositories.Company;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi_test.Application.Repositories;

public class CompanyRepositoryTest
{
    [Fact]
    public async Task GetCompanyByIdAsync_WithExistingCompany_ReturnsCompanyData()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new ()
            {
                Id = 1, 
                Name = "Company 1"
            }
        };

        var context = CreateDatabase(companyEntities);
        var companyRepository = new CompanyRepository(context);
        
        var expectedCompanyData = new CompanyDomain
        {
            Id = 1,
            Name = "Company 1",
            CreatedAt = DateTime.Now
        };
        
        // Act
        var companyData = await companyRepository.GetCompanyByIdAsync(1);
        
        // Assert
        expectedCompanyData.Should().BeEquivalentTo(companyData, options => options.Excluding(x => x!.CreatedAt));
    }
    
    [Fact]
    public async Task GetCompanyByIdAsync_WithNonExistingCompany_ReturnsNull()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new ()
            {
                Id = 1, 
                Name = "Company 1"
            }
        };

        var context = CreateDatabase(companyEntities);
        var companyRepository = new CompanyRepository(context);
        
        // Act
        var companyData = await companyRepository.GetCompanyByIdAsync(2);
        
        // Assert
        companyData.Should().BeNull();
    }
    
    [Fact]
    public async Task AddCompanyAsync_WithValidCompany_ReturnsCompanyId()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new ()
            {
                Id = 1, 
                Name = "Company1"
            }
        };

        var context = CreateDatabase(companyEntities);
        var companyRepository = new CompanyRepository(context);
        
        var newCompanyData = new CompanyCreation
        {
            Name = "Company 2"
        };
        
        // Act
        var companyId = await companyRepository.AddCompanyAsync(newCompanyData);
        
        // Assert
        companyId.Should().Be(2);
    }
    
    [Fact]
    public async Task UpdateCompanyAsync_WithExistingCompany_ReturnsUpdatedCompanyData()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new ()
            {
                Id = 1, 
                Name = "Company 1"
            }
        };

        var context = CreateDatabase(companyEntities);
        var companyRepository = new CompanyRepository(context);
        
        var updatedCompanyData = new CompanyUpdate()
        {
            Id = 1,
            Name = "Company 2"
        };
        
        // Act
        var companyData = await companyRepository.UpdateCompanyAsync(updatedCompanyData);
        
        // Assert
        updatedCompanyData.Should().BeEquivalentTo(companyData, options => options.Excluding(x => x!.CreatedAt));
    }
    
    [Fact]
    public async Task UpdateCompanyAsync_WithNonExistingCompany_ReturnsNull()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new ()
            {
                Id = 1, 
                Name = "Company 1"
            }
        };

        var context = CreateDatabase(companyEntities);
        var companyRepository = new CompanyRepository(context);
        
        var updatedCompanyData = new CompanyUpdate()
        {
            Id = 2,
            Name = "Company 2"
        };
        
        // Act
        var companyData = await companyRepository.UpdateCompanyAsync(updatedCompanyData);
        
        // Assert
        companyData.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteCompanyAsync_WithExistingCompany_ReturnsDeletedCompanyData()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new ()
            {
                Id = 1, 
                Name = "Company 1"
            }
        };

        var context = CreateDatabase(companyEntities);
        var companyRepository = new CompanyRepository(context);
        
        // Act
        var companyData = await companyRepository.DeleteCompanyAsync(1);
        
        // Assert
        companyData.Should().NotBeNull();
    }
    
    [Fact]
    public async Task DeleteCompanyAsync_WithNonExistingCompany_ReturnsNull()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new ()
            {
                Id = 1, 
                Name = "Company 1"
            }
        };

        var context = CreateDatabase(companyEntities);
        var companyRepository = new CompanyRepository(context);
        
        // Act
        var companyData = await companyRepository.DeleteCompanyAsync(2);
        
        // Assert
        companyData.Should().BeNull();
    }
    
    private static MarketTrackerDataContext CreateDatabase(IEnumerable<CompanyEntity> companyEntities)
    {
        DbContextOptions<MarketTrackerDataContext> options = new DbContextOptionsBuilder<MarketTrackerDataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var databaseContext = new MarketTrackerDataContext(options);
        databaseContext.Company.AddRange(companyEntities);
        databaseContext.SaveChanges();
        databaseContext.Database.EnsureCreated();
        return databaseContext;
    }
}