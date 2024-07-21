using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Http.Controllers.Market;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Company;
using market_tracker_webapi.Application.Service.Operations.Market.Company;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace market_tracker_webapi_test.Application.Controllers;


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
    public async Task GetCompaniesAsync_ReturnsCompanies()
    {
        // Arrange
        var companies = new List<Company>()
        {
            new(1, "Company 1", "https://company1.com/logo.png", new DateTime(2024, 01, 01, 0 ,0 ,0, DateTimeKind.Unspecified)),
        };
        _companyServiceMock
            .Setup(service => service.GetCompaniesAsync())
            .ReturnsAsync(companies);
        
        // Act
        var result = await _companyController.GetCompaniesAsync();
        
        // Assert
        var actionResult = Assert.IsType<ActionResult<CollectionOutputModel<CompanyOutputModel>>>(result);
        var collectionOutputModel = Assert.IsType<CollectionOutputModel<CompanyOutputModel>>(actionResult.Value);
        
        collectionOutputModel.Should().BeEquivalentTo(new CollectionOutputModel<CompanyOutputModel>(companies.Select(c => c.ToOutputModel())));
    }
    
    [Fact]
    public async Task GetCompanyByIdAsync_ReturnsCompany()
    {
        // Arrange
        var company = new Company(1, "Company 1", "https://company1.com/logo.png", new DateTime(2024, 01, 01, 0 ,0 ,0, DateTimeKind.Unspecified));
        _companyServiceMock
            .Setup(service => service.GetCompanyByIdAsync(1))
            .ReturnsAsync(company);
        
        // Act
        var result = await _companyController.GetCompanyByIdAsync(1);
        
        // Assert
        var actionResult = Assert.IsType<ActionResult<CompanyOutputModel>>(result);
        var companyOutputModel = Assert.IsType<CompanyOutputModel>(actionResult.Value);
        
        companyOutputModel.Should().BeEquivalentTo(company.ToOutputModel());
    }
    
    [Fact]
    public async Task UpdateCompanyAsync_ReturnsUpdatedCompany()
    {
        // Arrange
        var company = new Company(1, "Company 1", "https://company1.com/logo.png", new DateTime(2024, 01, 01, 0 ,0 ,0, DateTimeKind.Unspecified));
        _companyServiceMock
            .Setup(service => service.UpdateCompanyAsync(1, "Company 2"))
            .ReturnsAsync(company);
        
        // Act
        var result = await _companyController.UpdateCompanyAsync(1, new CompanyUpdateInputModel("Company 2"));
        
        // Assert
        var actionResult = Assert.IsType<ActionResult<CompanyOutputModel>>(result);
        var companyOutputModel = Assert.IsType<CompanyOutputModel>(actionResult.Value);
        
        companyOutputModel.Should().BeEquivalentTo(company.ToOutputModel());
    }
    
    [Fact]
    public async Task DeleteCompanyAsync_ReturnsNoContent()
    {
        // Arrange
        _companyServiceMock
            .Setup(service => service.DeleteCompanyAsync(1))
            .ReturnsAsync(new CompanyId(1));
        
        // Act
        var result = await _companyController.DeleteCompanyAsync(1);
        
        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
