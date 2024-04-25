using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository.Operations.Company;
using market_tracker_webapi.Application.Service.Errors.Company;
using market_tracker_webapi.Application.Service.Operations.Company;
using Moq;

namespace market_tracker_webapi_test.Application.Service;

public class CompanyServiceTest
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;

    private readonly CompanyService _companyService;

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
        var companies = new List<Company>
        {
            new()
            {
                Id = 1,
                Name = "Company 1",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new()
            {
                Id = 2,
                Name = "Company 2",
                CreatedAt = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };

        _companyRepositoryMock.Setup(x => x.GetCompaniesAsync()).ReturnsAsync(companies);

        // Act
        var result = await _companyService.GetCompaniesAsync();

        // Assert
        result.Value.Should().BeEquivalentTo(new CollectionOutputModel<Company>(companies));
    }

    [Fact]
    public async Task GetCompanyByIdAsync_ReturnsCompany()
    {
        // Arrange
        var company = new Company
        {
            Id = 1,
            Name = "Company 1",
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
        };

        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(company);

        // Act
        var result = await _companyService.GetCompanyByIdAsync(1);

        // Assert
        result.Value.Should().BeEquivalentTo(company);
    }

    [Fact]
    public async Task GetCompanyByIdAsync_ReturnsCompanyByIdNotFound()
    {
        // Arrange
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Company)null!);

        // Act
        var result = await _companyService.GetCompanyByIdAsync(1);

        // Assert
        result.Error.Should().BeEquivalentTo(new CompanyFetchingError.CompanyByIdNotFound(1));
    }

    [Fact]
    public async Task GetCompanyByNameAsync_ReturnsCompany()
    {
        // Arrange
        var company = new Company
        {
            Id = 1,
            Name = "Company 1",
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
        };

        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(company);

        // Act
        var result = await _companyService.GetCompanyByNameAsync("Company 1");

        // Assert
        result.Value.Should().BeEquivalentTo(company);
    }

    [Fact]
    public async Task GetCompanyByNameAsync_ReturnsCompanyByNameNotFound()
    {
        // Arrange
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((Company)null!);

        // Act
        var result = await _companyService.GetCompanyByNameAsync("Company 1");

        // Assert
        result
            .Error.Should()
            .BeEquivalentTo(new CompanyFetchingError.CompanyByNameNotFound("Company 1"));
    }

    [Fact]
    public async Task AddCompanyAsync_ReturnsIdOutputModel()
    {
        // Arrange
        var companyName = "Company 1";

        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(It.IsAny<Company>());

        _companyRepositoryMock.Setup(x => x.AddCompanyAsync(It.IsAny<string>())).ReturnsAsync(1);

        // Act
        var result = await _companyService.AddCompanyAsync(companyName);

        // Assert
        result.Value.Id.Should().Be(1);
    }

    [Fact]
    public async Task AddCompanyAsync_ReturnsCompanyNameAlreadyExists()
    {
        // Arrange
        var companyName = "Company 1";

        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(
                new Company
                {
                    Id = 1,
                    Name = "Company 1",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
                }
            );

        // Act
        var result = await _companyService.AddCompanyAsync(companyName);

        // Assert
        result
            .Error.Should()
            .BeEquivalentTo(new CompanyCreationError.CompanyNameAlreadyExists(companyName));
    }

    [Fact]
    public async Task DeleteCompanyAsync_ReturnsIdOutputModel()
    {
        // Arrange
        var id = 1;

        _companyRepositoryMock
            .Setup(x => x.DeleteCompanyAsync(It.IsAny<int>()))
            .ReturnsAsync(
                new Company
                {
                    Id = 1,
                    Name = "Company 1",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
                }
            );

        // Act
        var result = await _companyService.DeleteCompanyAsync(id);

        // Assert
        result.Value.Id.Should().Be(1);
    }

    [Fact]
    public async Task DeleteCompanyAsync_ReturnsCompanyByIdNotFound()
    {
        // Arrange
        var id = 1;

        _companyRepositoryMock
            .Setup(x => x.DeleteCompanyAsync(It.IsAny<int>()))
            .ReturnsAsync((Company)null!);

        // Act
        var result = await _companyService.DeleteCompanyAsync(id);

        // Assert
        result.Error.Should().BeEquivalentTo(new CompanyFetchingError.CompanyByIdNotFound(id));
    }

    [Fact]
    public async Task UpdateCompanyAsync_ReturnsCompanyDomain()
    {
        // Arrange
        var id = 1;
        var companyName = "Company 1";

        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((Company)null!);

        _companyRepositoryMock
            .Setup(x => x.UpdateCompanyAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(
                new Company
                {
                    Id = 1,
                    Name = "Company 1",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
                }
            );

        // Act
        var result = await _companyService.UpdateCompanyAsync(id, companyName);

        // Assert
        result.Value.Id.Should().Be(1);
    }

    [Fact]
    public async Task UpdateCompanyAsync_ReturnsCompanyNameAlreadyExists()
    {
        // Arrange
        var id = 1;
        var companyName = "Company 1";

        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(
                new Company
                {
                    Id = 1,
                    Name = "Company 1",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
                }
            );

        // Act
        var result = await _companyService.UpdateCompanyAsync(id, companyName);

        // Assert
        result
            .Error.Should()
            .BeEquivalentTo(new CompanyCreationError.CompanyNameAlreadyExists(companyName));
    }

    [Fact]
    public async Task UpdateCompanyAsync_ReturnsCompanyByIdNotFound()
    {
        // Arrange
        var id = 1;
        var companyName = "Company 1";

        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((Company)null!);

        _companyRepositoryMock
            .Setup(x => x.UpdateCompanyAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync((Company)null!);

        // Act
        var result = await _companyService.UpdateCompanyAsync(id, companyName);

        // Assert
        result.Error.Should().BeEquivalentTo(new CompanyFetchingError.CompanyByIdNotFound(id));
    }
}
