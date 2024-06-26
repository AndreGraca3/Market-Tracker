﻿using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi_test.Application.Repository;

/*public class CompanyRepositoryTest
{
    [Fact]
    public async Task GetCompanyByIdAsync_WithExistingCompany_ReturnsCompanyData()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new() { Id = 1, Name = "Company 1" }
        };

        var context = CreateDatabase(companyEntities);
        var companyRepository = new CompanyRepository(context);

        var expectedCompanyData = new Company
        {
            Id = 1,
            Name = "Company 1",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var companyData = await companyRepository.GetCompanyByIdAsync(1);

        // Assert
        expectedCompanyData
            .Should()
            .BeEquivalentTo(companyData, options => options.Excluding(x => x!.CreatedAt));
    }

    [Fact]
    public async Task GetCompanyByIdAsync_WithNonExistingCompany_ReturnsNull()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new() { Id = 1, Name = "Company 1" }
        };

        var context = CreateDatabase(companyEntities);
        var companyRepository = new CompanyRepository(context);

        // Act
        var companyData = await companyRepository.GetCompanyByIdAsync(2);

        // Assert
        companyData.Should().BeNull();
    }

    [Fact]
    public async Task GetCompaniesAsync_WithExistingCompanies_ReturnsCompanyData()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new() { Id = 1, Name = "Company 1" },
            new() { Id = 2, Name = "Company 2" }
        };

        var context = CreateDatabase(companyEntities);
        var companyRepository = new CompanyRepository(context);

        var expectedCompanyData = new List<Company>
        {
            new()
            {
                Id = 1,
                Name = "Company 1",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = 2,
                Name = "Company 2",
                CreatedAt = DateTime.UtcNow
            }
        };

        // Act
        var companyData = await companyRepository.GetCompaniesAsync();

        // Assert
        expectedCompanyData
            .Should()
            .BeEquivalentTo(companyData, options => options.Excluding(p => p.CreatedAt));
    }

    [Fact]
    public async Task GetCompaniesAsync_WithNoCompanies_ReturnsEmptyList()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>();

        var context = CreateDatabase(companyEntities);
        var companyRepository = new CompanyRepository(context);

        // Act
        var companyData = await companyRepository.GetCompaniesAsync();

        // Assert
        companyData.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCompanyByNameAsync_WithExistingCompany_ReturnsCompanyData()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new() { Id = 1, Name = "Company 1" }
        };

        var context = CreateDatabase(companyEntities);
        var companyRepository = new CompanyRepository(context);

        var expectedCompanyData = new Company
        {
            Id = 1,
            Name = "Company 1",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var companyData = await companyRepository.GetCompanyByNameAsync("Company 1");

        // Assert
        expectedCompanyData
            .Should()
            .BeEquivalentTo(companyData, options => options.Excluding(x => x!.CreatedAt));
    }

    [Fact]
    public async Task GetCompanyByNameAsync_WithNonExistingCompany_ReturnsNull()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new() { Id = 1, Name = "Company 1" }
        };

        var context = CreateDatabase(companyEntities);
        var companyRepository = new CompanyRepository(context);

        // Act
        var companyData = await companyRepository.GetCompanyByNameAsync("Company 2");

        // Assert
        companyData.Should().BeNull();
    }

    [Fact]
    public async Task AddCompanyAsync_WithValidCompany_ReturnsCompanyId()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new() { Id = 1, Name = "Company1" }
        };

        var context = CreateDatabase(companyEntities);
        var companyRepository = new CompanyRepository(context);

        // Act
        var companyId = await companyRepository.AddCompanyAsync("Company 2");

        // Assert
        companyId.Should().Be(2);
    }

    [Fact]
    public async Task UpdateCompanyAsync_WithExistingCompany_ReturnsUpdatedCompanyData()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new() { Id = 1, Name = "Company 1" }
        };

        var context = CreateDatabase(companyEntities);
        var companyRepository = new CompanyRepository(context);

        var updatedCompanyData = new Company
        {
            Id = 1,
            Name = "Company 2",
            CreatedAt = DateTime.UtcNow
        };
        // Act
        var companyData = await companyRepository.UpdateCompanyAsync(
            updatedCompanyData.Id,
            updatedCompanyData.Name
        );

        // Assert
        updatedCompanyData
            .Should()
            .BeEquivalentTo(companyData, options => options.Excluding(x => x!.CreatedAt));
    }

    [Fact]
    public async Task UpdateCompanyAsync_WithNonExistingCompany_ReturnsNull()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new() { Id = 1, Name = "Company 1" }
        };

        var context = CreateDatabase(companyEntities);
        var companyRepository = new CompanyRepository(context);

        var updatedCompanyData = new Company()
        {
            Id = 2,
            Name = "Company 2",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var companyData = await companyRepository.UpdateCompanyAsync(
            updatedCompanyData.Id,
            updatedCompanyData.Name
        );

        // Assert
        companyData.Should().BeNull();
    }

    [Fact]
    public async Task DeleteCompanyAsync_WithExistingCompany_ReturnsDeletedCompanyData()
    {
        // Arrange
        var companyEntities = new List<CompanyEntity>
        {
            new() { Id = 1, Name = "Company 1" }
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
            new() { Id = 1, Name = "Company 1" }
        };

        var context = CreateDatabase(companyEntities);
        var companyRepository = new CompanyRepository(context);

        // Act
        var companyData = await companyRepository.DeleteCompanyAsync(2);

        // Assert
        companyData.Should().BeNull();
    }

    private static MarketTrackerDataContext CreateDatabase(
        IEnumerable<CompanyEntity> companyEntities
    )
    {
        DbContextOptions<MarketTrackerDataContext> options =
            new DbContextOptionsBuilder<MarketTrackerDataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        var databaseContext = new MarketTrackerDataContext(options);
        databaseContext.Company.AddRange(companyEntities);
        databaseContext.SaveChanges();
        databaseContext.Database.EnsureCreated();
        return databaseContext;
    }
}*/
