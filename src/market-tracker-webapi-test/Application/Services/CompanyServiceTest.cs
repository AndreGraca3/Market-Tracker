using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Repositories.Company;
using market_tracker_webapi.Application.Service.Errors.Company;
using market_tracker_webapi.Application.Service.Operations.Company;
using market_tracker_webapi.Application.Services.Errors.Company;
using market_tracker_webapi.Application.Services.Transaction;
using market_tracker_webapi.Application.Utils;
using Moq;

namespace market_tracker_webapi_test.Application.Services;

public class CompanyServiceTest
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    
    private readonly Mock<ITransactionManager> _transactionManagerMock;
    
    private readonly CompanyService _companyService;
    
    public CompanyServiceTest()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _transactionManagerMock = new Mock<ITransactionManager>();
        _companyService = new CompanyService(_companyRepositoryMock.Object, _transactionManagerMock.Object);
    }
    
    [Fact]
    public async Task GetCompaniesAsync_ReturnsCompanies()
    {
        // Arrange
        var companies = new List<CompanyDomain>
        {
            new()
            {
                Id = 1,
                Name = "Company 1",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new ()
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
        result.Should().BeEquivalentTo(companies);
    }
    
    [Fact]
    public async Task GetCompanyByIdAsync_ReturnsCompany()
    {
        // Arrange
        var company = new CompanyDomain
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
            .ReturnsAsync((CompanyDomain)null!);
        
        // Act
        var result = await _companyService.GetCompanyByIdAsync(1);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new CompanyFetchingError.CompanyByIdNotFound(1));
    }
    
    [Fact]
    public async Task AddCompanyAsync_ReturnsIdOutputModel()
    {
        // Arrange
        var companyName = "Company 1";

        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<Either<ICompanyError, IdOutputModel>>>>()))
            .ReturnsAsync(EitherExtensions.Success<ICompanyError, IdOutputModel>(
                new IdOutputModel
                {
                    Id = 1
                }
            ));
        
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(It.IsAny<CompanyDomain>());
        
        _companyRepositoryMock
            .Setup(x => x.AddCompanyAsync(It.IsAny<string>()))
            .ReturnsAsync(1);
        
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

        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<Either<ICompanyError, IdOutputModel>>>>()))
            .ReturnsAsync(EitherExtensions.Failure<ICompanyError, IdOutputModel>(
                new CompanyCreationError.CompanyNameAlreadyExists(companyName)
            ));
        
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new CompanyDomain
            {
                Id = 1,
                Name = "Company 1",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
            });
        
        // Act
        var result = await _companyService.AddCompanyAsync(companyName);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new CompanyCreationError.CompanyNameAlreadyExists(companyName));
    }
    
    [Fact]
    public async Task DeleteCompanyAsync_ReturnsIdOutputModel()
    {
        // Arrange
        var id = 1;

        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<Either<CompanyFetchingError, IdOutputModel>>>>()))
            .ReturnsAsync(EitherExtensions.Success<CompanyFetchingError, IdOutputModel>(
                new IdOutputModel
                {
                    Id = 1
                }
            ));
        
        _companyRepositoryMock
            .Setup(x => x.DeleteCompanyAsync(It.IsAny<int>()))
            .ReturnsAsync(new CompanyDomain
            {
                Id = 1,
                Name = "Company 1",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
            });
        
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

        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<Either<CompanyFetchingError, IdOutputModel>>>>()))
            .ReturnsAsync(EitherExtensions.Failure<CompanyFetchingError, IdOutputModel>(
                new CompanyFetchingError.CompanyByIdNotFound(id)
            ));
        
        _companyRepositoryMock
            .Setup(x => x.DeleteCompanyAsync(It.IsAny<int>()))
            .ReturnsAsync((CompanyDomain)null!);
        
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

        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<Either<ICompanyError, CompanyDomain>>>>()))
            .ReturnsAsync(EitherExtensions.Success<ICompanyError, CompanyDomain>(
                new CompanyDomain
                {
                    Id = 1,
                    Name = "Company 1",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
                }
            ));
        
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((CompanyDomain)null!);
        
        _companyRepositoryMock
            .Setup(x => x.UpdateCompanyAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(new CompanyDomain
            {
                Id = 1,
                Name = "Company 1",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
            });
        
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

        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<Either<ICompanyError, CompanyDomain>>>>()))
            .ReturnsAsync(EitherExtensions.Failure<ICompanyError, CompanyDomain>(
                new CompanyCreationError.CompanyNameAlreadyExists(companyName)
            ));
        
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new CompanyDomain
            {
                Id = 1,
                Name = "Company 1",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
            });
        
        // Act
        var result = await _companyService.UpdateCompanyAsync(id, companyName);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new CompanyCreationError.CompanyNameAlreadyExists(companyName));
    }
    
    [Fact]
    public async Task UpdateCompanyAsync_ReturnsCompanyByIdNotFound()
    {
        // Arrange
        var id = 1;
        var companyName = "Company 1";

        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<Either<ICompanyError, CompanyDomain>>>>()))
            .ReturnsAsync(EitherExtensions.Failure<ICompanyError, CompanyDomain>(
                new CompanyFetchingError.CompanyByIdNotFound(id)
            ));
        
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((CompanyDomain)null!);
        
        _companyRepositoryMock
            .Setup(x => x.UpdateCompanyAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync((CompanyDomain)null!);
        
        // Act
        var result = await _companyService.UpdateCompanyAsync(id, companyName);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new CompanyFetchingError.CompanyByIdNotFound(id));
    }
}