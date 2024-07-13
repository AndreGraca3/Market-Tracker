using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Repository.Market.Company;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi_test.Application.Repository.Market;

public class CompanyRepositoryTest
{
    [Fact]
    public async Task GetCompanyByIdAsync_WhenCompanyExists_ReturnsCompany()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new CompanyEntity { Id = 1, Name = "companyName", CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified), LogoUrl = "logoUrl1"},
            new CompanyEntity { Id = 2, Name = "companyName2", CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified), LogoUrl = "logoUrl2" }
        };
        
        var context = DbHelper.CreateDatabase(companyEntities);
        
        var companyRepository = new CompanyRepository(context);
        
        // Act
        var actualCompany = await companyRepository.GetCompanyByIdAsync(1);
        
        // Assert
        var expectedCompany = new Company(1, "companyName", "logoUrl1", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified));
        actualCompany.Should().BeEquivalentTo(expectedCompany);
    }
    
    [Fact]
    public async Task GetCompanyByIdAsync_WhenCompanyDoesNotExist_ReturnsNull()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new CompanyEntity { Id = 1, Name = "companyName", CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified), LogoUrl = "logoUrl1"},
            new CompanyEntity { Id = 2, Name = "companyName2", CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified), LogoUrl = "logoUrl2" }
        };
        
        var context = DbHelper.CreateDatabase(companyEntities);
        
        var companyRepository = new CompanyRepository(context);
        
        // Act
        var actualCompany = await companyRepository.GetCompanyByIdAsync(3);
        
        // Assert
        actualCompany.Should().BeNull();
    }
    
    [Fact]
    public async Task GetCompanyByNameAsync_WhenCompanyExists_ReturnsCompany()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new CompanyEntity { Id = 1, Name = "companyName", CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified), LogoUrl = "logoUrl1"},
            new CompanyEntity { Id = 2, Name = "companyName2", CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified), LogoUrl = "logoUrl2" }
        };
        
        var context = DbHelper.CreateDatabase(companyEntities);
        
        var companyRepository = new CompanyRepository(context);
        
        // Act
        var actualCompany = await companyRepository.GetCompanyByNameAsync("companyName");
        
        // Assert
        var expectedCompany = new Company(1, "companyName", "logoUrl1", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified));
        actualCompany.Should().BeEquivalentTo(expectedCompany);
    }
    
    [Fact]
    public async Task GetCompanyByNameAsync_WhenCompanyDoesNotExist_ReturnsNull()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new CompanyEntity { Id = 1, Name = "companyName", CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified), LogoUrl = "logoUrl1"},
            new CompanyEntity { Id = 2, Name = "companyName2", CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified), LogoUrl = "logoUrl2" }
        };
        
        var context = DbHelper.CreateDatabase(companyEntities);
        
        var companyRepository = new CompanyRepository(context);
        
        // Act
        var actualCompany = await companyRepository.GetCompanyByNameAsync("companyName3");
        
        // Assert
        actualCompany.Should().BeNull();
    }
    
    [Fact]
    public async Task GetCompaniesAsync_WhenCompaniesExist_ReturnsCompanies()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new CompanyEntity { Id = 1, Name = "companyName", CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified), LogoUrl = "logoUrl1"},
            new CompanyEntity { Id = 2, Name = "companyName2", CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified), LogoUrl = "logoUrl2" }
        };
        
        var context = DbHelper.CreateDatabase(companyEntities);
        
        var companyRepository = new CompanyRepository(context);
        
        // Act
        var actualCompanies = await companyRepository.GetCompaniesAsync();
        
        // Assert
        var expectedCompanies = new List<Company>
        {
            new(1, "companyName", "logoUrl1", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)),
            new(2, "companyName2", "logoUrl2", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified))
        };
        actualCompanies.Should().BeEquivalentTo(expectedCompanies);
    }
    
    [Fact]
    public async Task AddCompanyAsync_WhenCompanyDoesNotExist_AddsCompany()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new CompanyEntity { Id = 1, Name = "companyName", CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified), LogoUrl = "logoUrl1"},
            new CompanyEntity { Id = 2, Name = "companyName2", CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified), LogoUrl = "logoUrl2" }
        };
        
        var context = DbHelper.CreateDatabase(companyEntities);
        
        var companyRepository = new CompanyRepository(context);
        
        var newCompany = new Company(3, "companyName3", "logoUrl3", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified));
        
        // Act
        await companyRepository.AddCompanyAsync("companyName3", "logoUrl3");
        
        // Assert
        var actualCompany = await companyRepository.GetCompanyByIdAsync(3);
        actualCompany.Should().BeEquivalentTo(newCompany, x => x.Excluding(y => y.CreatedAt));
    }
    
    [Fact]
    public async Task UpdateCompanyAsync_WhenCompanyExists_UpdatesCompany()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new CompanyEntity { Id = 1, Name = "companyName", CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified), LogoUrl = "logoUrl1"},
            new CompanyEntity { Id = 2, Name = "companyName2", CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified), LogoUrl = "logoUrl2" }
        };
        
        var context = DbHelper.CreateDatabase(companyEntities);
        
        var companyRepository = new CompanyRepository(context);
        
        var updatedCompany = new Company(1, "companyNameUpdated", "logoUrl1", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified));
        
        // Act
        await companyRepository.UpdateCompanyAsync(1, "companyNameUpdated");
        
        // Assert
        var actualCompany = await companyRepository.GetCompanyByIdAsync(1);
        actualCompany.Should().BeEquivalentTo(updatedCompany);
    }
    
    
}
