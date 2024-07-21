using FluentAssertions;
using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Http.Controllers.Market.Store;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Operator;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;
using market_tracker_webapi.Application.Service.Operations.Account.Auth.PreRegister;
using Moq;

namespace market_tracker_webapi_test.Application.Controllers;

public class StorePreRegisterControllerTest
{
    private readonly Mock<IPreRegistrationService> _preRegistrationService;
    
    private readonly StorePreRegisterController _controller;
    
    public StorePreRegisterControllerTest()
    {
        _preRegistrationService = new Mock<IPreRegistrationService>();
        _controller = new StorePreRegisterController(_preRegistrationService.Object);
    }
    
    [Fact]
    public async Task GetPendingOperatorsAsync_ShouldReturnOperators()
    {
        // Arrange
        var paginationResultOperator = new PaginatedResult<OperatorItem>(new List<OperatorItem>()
        {
            new (
                Guid.Parse("f1b1b3b1-1b1b-1b1b-1b1b-1b1b1b1b1b1b"),
                "Operator Name",
                "CompanyLogoUrl"
            )
        }, 1, 1, 1);
        
        _preRegistrationService.Setup(x => x.GetPreRegistrationsAsync(It.IsAny<bool?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(paginationResultOperator);
        
        // Act
        var paginationInputs = new PaginationInputs()
        {
            ItemsPerPage = 1,
            Page = 1
        };
        
        var result = await _controller.GetPendingOperatorsAsync(It.IsAny<bool?>(), paginationInputs);
        
        // Assert
        result.Should().BeEquivalentTo(paginationResultOperator, x => x.ExcludingMissingMembers());
    }
    
    [Fact]
    public async Task GetPendingOperatorByIdAsync_ShouldReturnOperator()
    {
        // Arrange
        var preRegistration = new PreRegistration(
            Guid.Parse("f1b1b3b1-1b1b-1b1b-1b1b-1b1b1b1b1b1b"),
            "Operator Name",
            "CompanyLogoUrl",
            1,
            "Store Name", 
            "Company Name", 
            "Company LogoUrl", 
            "Store Address", 
            "City Name",
            "Document",
            new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
            true
        );
        
        _preRegistrationService.Setup(x => x.GetPreRegistrationByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(preRegistration);
        
        // Act
        var result = await _controller.GetPendingOperatorByIdAsync(It.IsAny<Guid>());
        
        // Assert
        result.Should().BeEquivalentTo(preRegistration, x => x.ExcludingMissingMembers());
    }

    [Fact]
    public async Task AddPreRegisterAsync_ShouldReturnPreRegistrationCode()
    {
        // Arrange
        var preRegistrationCode = new PreRegistrationCode(Guid.Parse("f1b1b3b1-1b1b-1b1b-1b1b-1b1b1b1b1b1b"));

        _preRegistrationService
            .Setup(x => x.AddPreRegistrationAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            ))
            .ReturnsAsync(preRegistrationCode);
        
        // Act
        var preRegistrationCreationInputModel = new PreRegistrationCreationInputModel(
            "Operator Name",
            "Email",
            1,
            "Store Name",
            "Store Address",
            "Company Name",
            "Company LogoUrl",
            "City Name",
            "Document"
            );
        
        var result = await _controller.AddPreRegisterAsync(preRegistrationCreationInputModel);
        
        // Assert
        result.Value.Should().BeEquivalentTo(preRegistrationCode, x => x.ExcludingMissingMembers());
    }
    
    [Fact]
    public async Task UpdateStatePreRegistrationByIdAsync_ShouldReturnPreRegistrationCode()
    {
        // Arrange
        var preRegistrationCode = new PreRegistrationCode(Guid.Parse("f1b1b3b1-1b1b-1b1b-1b1b-1b1b1b1b1b1b"));

        _preRegistrationService
            .Setup(x => x.UpdatePreRegistrationByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<bool>()
            ))
            .ReturnsAsync(preRegistrationCode);
        
        // Act
        var preRegistrationValidationInputModel = new PreRegistrationValidationInputModel(true);
        
        var result = await _controller.UpdateStatePreRegistrationByIdAsync(It.IsAny<Guid>(), preRegistrationValidationInputModel);
        
        // Assert
        result.Value.Should().BeEquivalentTo(preRegistrationCode, x => x.ExcludingMissingMembers());
    }
}