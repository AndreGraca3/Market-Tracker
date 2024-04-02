using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Controllers;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Service.Errors.Company;
using market_tracker_webapi.Application.Service.Operations.Company;
using market_tracker_webapi.Application.Services.Errors.Company;
using market_tracker_webapi.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace market_tracker_webapi_test.Controllers;

public class CompanyControllerTest
{
    private readonly Mock<ICompanyService> _companyServiceMock;
    private readonly CompanyController _companyController;

    public CompanyControllerTest()
    {
        _companyServiceMock = new Mock<ICompanyService>();
        _companyController = new CompanyController(_companyServiceMock.Object);
    }

    [Fact]
    public async Task GetCompaniesAsync_RespondsWith_Ok_ReturnsObjectAsync()
    {
        // Expected Arrange
        var expectedCompanies = new List<Company>
        {
            new()
            {
                Id = 1,
                Name = "Company 1",
                CreatedAt = DateTime.Now
            },
            new()
            {
                Id = 2,
                Name = "Company 2",
                CreatedAt = DateTime.Now
            }
        };

        // Service Arrange
        _companyServiceMock
            .Setup(service => service.GetCompaniesAsync())
            .ReturnsAsync(new CollectionOutputModel(expectedCompanies));

        // Act
        var actual = await _companyController.GetCompaniesAsync();

        // Assert
        var result = Assert.IsType<OkObjectResult>(actual.Result);
        var actualCompaniesCollection = Assert.IsAssignableFrom<CollectionOutputModel>(
            result.Value
        );
        actualCompaniesCollection
            .Should()
            .BeEquivalentTo(new CollectionOutputModel(expectedCompanies));
        actualCompaniesCollection.Results.Should().BeEquivalentTo(expectedCompanies);
    }

    [Fact]
    public async Task GetCompanyByIdAsync_RespondsWith_Ok_ReturnsObjectAsync()
    {
        // Expected Arrange
        var expectedCompany = new Company
        {
            Id = 1,
            Name = "Company 1",
            CreatedAt = DateTime.Now
        };

        // Service Arrange
        _companyServiceMock
            .Setup(service => service.GetCompanyByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(EitherExtensions.Success<CompanyFetchingError, Company>(expectedCompany));

        // Act
        var actual = await _companyController.GetCompanyByIdAsync(It.IsAny<int>());

        // Assert
        OkObjectResult result = Assert.IsType<OkObjectResult>(actual.Result);
        Company company = Assert.IsAssignableFrom<Company>(result.Value);
        company.Should().BeEquivalentTo(expectedCompany);
    }

    [Fact]
    public async Task GetCompanyByIdAsync_RespondsWith_NotFound_ReturnsObjectAsync() // Ok -> NotFound
    {
        // Service Arrange
        _companyServiceMock
            .Setup(service => service.GetCompanyByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(
                EitherExtensions.Failure<CompanyFetchingError, Company>(
                    new CompanyFetchingError.CompanyByIdNotFound(It.IsAny<int>())
                )
            );

        // Act
        var actual = await _companyController.GetCompanyByIdAsync(It.IsAny<int>());

        // Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        CompanyProblem.CompanyByIdNotFound problem =
            Assert.IsAssignableFrom<CompanyProblem.CompanyByIdNotFound>(result.Value);
        problem
            .Data.Should()
            .BeEquivalentTo(new CompanyFetchingError.CompanyByIdNotFound(It.IsAny<int>()));
    }

    [Fact]
    public async Task AddCompanyAsync_RespondsWith_Created_ReturnsObjectAsync() // Ok -> Created
    {
        // Expected Arrange
        var expectedId = new IdOutputModel(1);

        // Service Arrange
        _companyServiceMock
            .Setup(service => service.AddCompanyAsync(It.IsAny<string>()))
            .ReturnsAsync(EitherExtensions.Success<ICompanyError, IdOutputModel>(expectedId));

        // Act
        var actual = await _companyController.AddCompanyAsync(
            new CompanyCreationInputModel { CompanyName = It.IsAny<string>() }
        );

        // Assert
        OkObjectResult result = Assert.IsType<OkObjectResult>(actual.Result);
        IdOutputModel idOutputModel = Assert.IsAssignableFrom<IdOutputModel>(result.Value);
        idOutputModel.Should().BeEquivalentTo(expectedId);
    }

    [Fact]
    public async Task AddCompanyAsync_RespondsWith_Conflict_ReturnsObjectAsync()
    {
        // Service Arrange
        _companyServiceMock
            .Setup(service => service.AddCompanyAsync(It.IsAny<string>()))
            .ReturnsAsync(
                EitherExtensions.Failure<ICompanyError, IdOutputModel>(
                    new CompanyCreationError.CompanyNameAlreadyExists(It.IsAny<string>())
                )
            );

        // Act
        var actual = await _companyController.AddCompanyAsync(
            new CompanyCreationInputModel { CompanyName = It.IsAny<string>() }
        );

        // Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        CompanyProblem.CompanyNameAlreadyExists problem =
            Assert.IsAssignableFrom<CompanyProblem.CompanyNameAlreadyExists>(result.Value);
        problem
            .Data.Should()
            .BeEquivalentTo(new CompanyCreationError.CompanyNameAlreadyExists(It.IsAny<string>()));
    }

    [Fact]
    public async Task UpdateCompanyAsync_RespondsWith_Ok_ReturnsObjectAsync()
    {
        // Expected Arrange
        var expectedCompany = new Company
        {
            Id = 1,
            Name = "Company 1",
            CreatedAt = DateTime.Now
        };

        // Service Arrange
        _companyServiceMock
            .Setup(service => service.UpdateCompanyAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(EitherExtensions.Success<ICompanyError, Company>(expectedCompany));

        // Act
        var actual = await _companyController.UpdateCompanyAsync(
            It.IsAny<int>(),
            new CompanyUpdateInputModel { CompanyName = It.IsAny<string>() }
        );

        // Assert
        OkObjectResult result = Assert.IsType<OkObjectResult>(actual.Result);
        Company company = Assert.IsAssignableFrom<Company>(result.Value);
        company.Should().BeEquivalentTo(expectedCompany);
    }

    [Fact]
    public async Task UpdateCompanyAsync_RespondsWith_NotFound_ReturnsObjectAsync() // Ok -> NotFound
    {
        // Service Arrange
        _companyServiceMock
            .Setup(service => service.UpdateCompanyAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(
                EitherExtensions.Failure<ICompanyError, Company>(
                    new CompanyFetchingError.CompanyByIdNotFound(It.IsAny<int>())
                )
            );

        // Act
        var actual = await _companyController.UpdateCompanyAsync(
            It.IsAny<int>(),
            new CompanyUpdateInputModel { CompanyName = It.IsAny<string>() }
        );

        // Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        CompanyProblem.CompanyByIdNotFound problem =
            Assert.IsAssignableFrom<CompanyProblem.CompanyByIdNotFound>(result.Value);
        problem
            .Data.Should()
            .BeEquivalentTo(new CompanyFetchingError.CompanyByIdNotFound(It.IsAny<int>()));
    }

    [Fact]
    public async Task UpdateCompanyAsync_RespondsWith_Conflict_ReturnsObjectAsync() // Ok -> Conflict
    {
        // Service Arrange
        _companyServiceMock
            .Setup(service => service.UpdateCompanyAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(
                EitherExtensions.Failure<ICompanyError, Company>(
                    new CompanyCreationError.CompanyNameAlreadyExists(It.IsAny<string>())
                )
            );

        // Act
        var actual = await _companyController.UpdateCompanyAsync(
            It.IsAny<int>(),
            new CompanyUpdateInputModel { CompanyName = It.IsAny<string>() }
        );

        // Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        CompanyProblem.CompanyNameAlreadyExists problem =
            Assert.IsAssignableFrom<CompanyProblem.CompanyNameAlreadyExists>(result.Value);
        problem
            .Data.Should()
            .BeEquivalentTo(new CompanyCreationError.CompanyNameAlreadyExists(It.IsAny<string>()));
    }

    [Fact]
    public async Task DeleteCompanyAsync_RespondsWith_Ok_ReturnsObjectAsync()
    {
        // Expected Arrange
        var expectedId = new IdOutputModel(1);

        // Service Arrange
        _companyServiceMock
            .Setup(service => service.DeleteCompanyAsync(It.IsAny<int>()))
            .ReturnsAsync(
                EitherExtensions.Success<CompanyFetchingError, IdOutputModel>(expectedId)
            );

        // Act
        var actual = await _companyController.DeleteCompanyAsync(It.IsAny<int>());

        // Assert
        OkObjectResult result = Assert.IsType<OkObjectResult>(actual.Result);
        IdOutputModel idOutputModel = Assert.IsAssignableFrom<IdOutputModel>(result.Value);
        idOutputModel.Should().BeEquivalentTo(expectedId);
    }

    [Fact]
    public async Task DeleteCompanyAsync_RespondsWith_NotFound_ReturnsObjectAsync() // Ok -> NotFound
    {
        // Service Arrange
        _companyServiceMock
            .Setup(service => service.DeleteCompanyAsync(It.IsAny<int>()))
            .ReturnsAsync(
                EitherExtensions.Failure<CompanyFetchingError, IdOutputModel>(
                    new CompanyFetchingError.CompanyByIdNotFound(It.IsAny<int>())
                )
            );

        // Act
        var actual = await _companyController.DeleteCompanyAsync(It.IsAny<int>());

        // Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        CompanyProblem.CompanyByIdNotFound problem =
            Assert.IsAssignableFrom<CompanyProblem.CompanyByIdNotFound>(result.Value);
        problem
            .Data.Should()
            .BeEquivalentTo(new CompanyFetchingError.CompanyByIdNotFound(It.IsAny<int>()));
    }
}
