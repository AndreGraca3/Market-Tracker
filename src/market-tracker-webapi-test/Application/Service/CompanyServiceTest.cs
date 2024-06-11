using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Repository.Market.Company;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Company;
using market_tracker_webapi.Application.Service.Operations.Market.Company;
using Moq;

namespace market_tracker_webapi_test.Application.Service;

public class CompanyServiceTest
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;

    private readonly CompanyService _companyService;

    private readonly List<Company> _companies =
    [
        new Company(
            1,
            "Company 1",
            "company1_logo",
            new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
        ),

        new Company(
            2,
            "Company 2",
            "company2_logo",
            new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Unspecified)
        )
    ];

    public CompanyServiceTest()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _companyService = new CompanyService(
            _companyRepositoryMock.Object,
            new MockedTransactionManager()
        );
    }

    [Fact]
    public async Task GetCompaniesAsync_ReturnsCompaniesCollectionOutputModel()
    {
        // Arrange
        _companyRepositoryMock.Setup(x => x.GetCompaniesAsync()).ReturnsAsync(_companies);

        // Act
        var result = await _companyService.GetCompaniesAsync();

        // Assert
        result.Should().BeEquivalentTo(_companies);
    }

    [Fact]
    public async Task GetCompanyByIdAsync_ReturnsCompany()
    {
        // Arrange
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(_companies[0]);

        // Act
        var result = await _companyService.GetCompanyByIdAsync(1);

        // Assert
        result.Should().BeEquivalentTo(_companies[0]);
    }

    [Fact]
    public async Task GetCompanyByIdAsync_ReturnsCompanyByIdNotFound()
    {
        // Arrange
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Company)null!);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _companyService.GetCompanyByIdAsync(1));

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(new CompanyFetchingError.CompanyByIdNotFound(1));
    }

    [Fact]
    public async Task GetCompanyByNameAsync_ReturnsCompany()
    {
        // Arrange
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(_companies[0]);

        // Act
        var result = await _companyService.GetCompanyByNameAsync("Company 1");

        // Assert
        result.Should().BeEquivalentTo(_companies[0]);
    }

    [Fact]
    public async Task GetCompanyByNameAsync_ReturnsCompanyByNameNotFound()
    {
        // Arrange
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((Company)null!);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _companyService.GetCompanyByNameAsync("Company 1"));

        // Assert
        result
            .ServiceError
            .Should()
            .BeEquivalentTo(new CompanyFetchingError.CompanyByNameNotFound("Company 1"));
    }

    [Fact]
    public async Task AddCompanyAsync_ReturnsIdOutputModel()
    {
        // Arrange
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(It.IsAny<Company>());

        _companyRepositoryMock
            .Setup(x => x.AddCompanyAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new CompanyId(1));

        // Act
        var result = await _companyService.AddCompanyAsync("Company 1", "company1_logo");

        // Assert
        result.Should().BeEquivalentTo(new CompanyId(1));
    }

    [Fact]
    public async Task AddCompanyAsync_ReturnsCompanyNameAlreadyExists()
    {
        // Arrange
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(_companies[0]);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _companyService.AddCompanyAsync(_companies[0].Name, _companies[0].LogoUrl));

        // Assert
        result
            .ServiceError
            .Should()
            .BeEquivalentTo(new CompanyCreationError.CompanyNameAlreadyExists(_companies[0].Name));
    }

    [Fact]
    public async Task DeleteCompanyAsync_ReturnsCompanyId()
    {
        // Arrange
        _companyRepositoryMock
            .Setup(x => x.DeleteCompanyAsync(It.IsAny<int>()))
            .ReturnsAsync(_companies[0]);

        // Act
        var result = await _companyService.DeleteCompanyAsync(_companies[0].Id.Value);

        // Assert
        result.Should().BeEquivalentTo(_companies[0].Id);
    }

    [Fact]
    public async Task DeleteCompanyAsync_ReturnsCompanyByIdNotFound()
    {
        // Arrange
        _companyRepositoryMock
            .Setup(x => x.DeleteCompanyAsync(It.IsAny<int>()))
            .ReturnsAsync((Company)null!);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _companyService.DeleteCompanyAsync(_companies[0].Id.Value));

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(new CompanyFetchingError.CompanyByIdNotFound(_companies[0].Id.Value));
    }

    [Fact]
    public async Task UpdateCompanyAsync_ReturnsCompany()
    {
        // Arrange
        Company newCompany = new Company(
            _companies[0].Id.Value,
            "new_company_name",
            _companies[0].LogoUrl,
            _companies[0].CreatedAt
        );
        
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((Company)null!);

        _companyRepositoryMock
            .Setup(x => x.UpdateCompanyAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(newCompany);

        // Act
        var result = await _companyService.UpdateCompanyAsync(_companies[0].Id.Value, newCompany.Name);

        // Assert
        result.Should().BeEquivalentTo(newCompany);
    }

    [Fact]
    public async Task UpdateCompanyAsync_ReturnsCompanyNameAlreadyExists()
    {
        // Arrange
        var newCompanyName = "new_company_name";
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(_companies[0]);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _companyService.UpdateCompanyAsync(_companies[0].Id.Value, newCompanyName));

        // Assert
        result
            .ServiceError.Should()
            .BeEquivalentTo(new CompanyCreationError.CompanyNameAlreadyExists(newCompanyName));
    }

    [Fact]
    public async Task UpdateCompanyAsync_ReturnsCompanyByIdNotFound()
    {
        // Arrange
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((Company)null!);

        _companyRepositoryMock
            .Setup(x => x.UpdateCompanyAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync((Company)null!);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () => 
            await _companyService.UpdateCompanyAsync(_companies[0].Id.Value, "new_company_name"));

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(new CompanyFetchingError.CompanyByIdNotFound(_companies[0].Id.Value));
    }
}