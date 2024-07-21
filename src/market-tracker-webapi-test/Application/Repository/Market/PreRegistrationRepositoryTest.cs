using FluentAssertions;
using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Repository.Market.Store.PreRegister;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Store;

namespace market_tracker_webapi_test.Application.Repository.Market;

public class PreRegistrationRepositoryTest
{
    [Fact]
    public async Task GetPreRegistersAsync_ReturnsPaginatedResult()
    {
        // Arrange
        var preRegisterEntities = new List<PreRegistrationEntity>
        {
            new PreRegistrationEntity
            {
                Code = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), 
                OperatorName = "operatorName", 
                Email = "email",
                PhoneNumber = 911111111,
                StoreAddress = "storeAddress",
                CompanyName = "companyName",
                StoreName = "storeName",
                CompanyLogoUrl = "companyLogoUrl",
                CityName = "cityName",
                Document = "document",
                IsApproved = true,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(preRegisterEntities);
        
        var preRegisterRepository = new PreRegistrationRepository(context);
        
        // Act
        var actualPreRegisters = await preRegisterRepository.GetPreRegistersAsync(null, 0, 2);
        
        // Assert
        var expectedOperatorItem = new OperatorItem(Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), "operatorName", "companyLogoUrl");
        
        actualPreRegisters.Should().BeOfType<PaginatedResult<OperatorItem>>();
        actualPreRegisters.Should().BeEquivalentTo(expectedOperatorItem, x => x.ExcludingMissingMembers());
    }
    
    [Fact]
    public async Task GetPreRegisterByIdAsync_WhenPreRegisterExists_ReturnsPreRegister()
    {
        // Arrange
        var preRegisterEntities = new List<PreRegistrationEntity>
        {
            new PreRegistrationEntity
            {
                Code = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), 
                OperatorName = "operatorName", 
                Email = "email",
                PhoneNumber = 911111111,
                StoreAddress = "storeAddress",
                CompanyName = "companyName",
                StoreName = "storeName",
                CompanyLogoUrl = "companyLogoUrl",
                CityName = "cityName",
                Document = "document",
                IsApproved = true,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(preRegisterEntities);
        
        var preRegisterRepository = new PreRegistrationRepository(context);
        
        // Act
        var actualPreRegister = await preRegisterRepository.GetPreRegisterByIdAsync(Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"));
        
        // Assert
        var expectedPreRegister = new PreRegistration(Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), "operatorName", "email", 911111111, "storeName", "companyName", "companyLogoUrl", "storeAddress", "cityName", "document", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified), true);
        actualPreRegister.Should().BeEquivalentTo(expectedPreRegister);
    }
    
    [Fact]
    public async Task GetPreRegisterByEmail_WhenPreRegisterExists_ReturnsPreRegister()
    {
        // Arrange
        var preRegisterEntities = new List<PreRegistrationEntity>
        {
            new PreRegistrationEntity
            {
                Code = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), 
                OperatorName = "operatorName", 
                Email = "email",
                PhoneNumber = 911111111,
                StoreAddress = "storeAddress",
                CompanyName = "companyName",
                StoreName = "storeName",
                CompanyLogoUrl = "companyLogoUrl",
                CityName = "cityName",
                Document = "document",
                IsApproved = true,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(preRegisterEntities);
        
        var preRegisterRepository = new PreRegistrationRepository(context);
        
        // Act
        var actualPreRegister = await preRegisterRepository.GetPreRegisterByEmail("email");
        
        // Assert
        var expectedPreRegister = new PreRegistration(Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), "operatorName", "email", 911111111, "storeName", "companyName", "companyLogoUrl", "storeAddress", "cityName", "document", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified), true);
        actualPreRegister.Should().BeEquivalentTo(expectedPreRegister);
    }
    
    [Fact]
    public async Task GetPreRegisterByEmail_WhenPreRegisterDoesNotExist_ReturnsNull()
    {
        // Arrange
        var preRegisterEntities = new List<PreRegistrationEntity>
        {
            new PreRegistrationEntity
            {
                Code = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), 
                OperatorName = "operatorName", 
                Email = "email",
                PhoneNumber = 911111111,
                StoreAddress = "storeAddress",
                CompanyName = "companyName",
                StoreName = "storeName",
                CompanyLogoUrl = "companyLogoUrl",
                CityName = "cityName",
                Document = "document",
                IsApproved = true,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(preRegisterEntities);
        
        var preRegisterRepository = new PreRegistrationRepository(context);
        
        // Act
        var actualPreRegister = await preRegisterRepository.GetPreRegisterByEmail("email2");
        
        // Assert
        actualPreRegister.Should().BeNull();
    }
    
    [Fact]
    public async Task CreatePreRegisterAsync_ReturnsPreRegisterCode()
    {
        // Arrange
        var preRegisterEntities = new List<PreRegistrationEntity>
        {
            new PreRegistrationEntity
            {
                Code = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), 
                OperatorName = "operatorName", 
                Email = "email",
                PhoneNumber = 911111111,
                StoreAddress = "storeAddress",
                CompanyName = "companyName",
                StoreName = "storeName",
                CompanyLogoUrl = "companyLogoUrl",
                CityName = "cityName",
                Document = "document",
                IsApproved = true,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(preRegisterEntities);
        
        var preRegisterRepository = new PreRegistrationRepository(context);
        
        // Act
        var actualPreRegisterCode = await preRegisterRepository.CreatePreRegisterAsync("operatorName", "email", 911111111, "storeName", "storeAddress", "companyName", "companyLogoUrl", "cityName", "document");
        
        // Assert   
        context.PreRegister.Count().Should().Be(2);
    }
    
    [Fact]
    public async Task UpdatePreRegisterById_WhenPreRegisterExists_ReturnsPreRegister()
    {
        // Arrange
        var preRegisterEntities = new List<PreRegistrationEntity>
        {
            new PreRegistrationEntity
            {
                Code = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), 
                OperatorName = "operatorName", 
                Email = "email",
                PhoneNumber = 911111111,
                StoreAddress = "storeAddress",
                CompanyName = "companyName",
                StoreName = "storeName",
                CompanyLogoUrl = "companyLogoUrl",
                CityName = "cityName",
                Document = "document",
                IsApproved = true,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(preRegisterEntities);
        
        var preRegisterRepository = new PreRegistrationRepository(context);
        
        // Act
        var actualPreRegister = await preRegisterRepository.UpdatePreRegistrationById(Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), false);
        
        // Assert
        var expectedPreRegister = new PreRegistration(Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), "operatorName", "email", 911111111, "storeName", "companyName", "companyLogoUrl", "storeAddress", "cityName", "document", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified), false);
        actualPreRegister.Should().BeEquivalentTo(expectedPreRegister, x => x.Excluding(y => y.Code));
    }
    
    [Fact]
    public async Task UpdatePreRegisterById_WhenPreRegisterDoesNotExist_ReturnsNull()
    {
        // Arrange
        var preRegisterEntities = new List<PreRegistrationEntity>
        {
            new PreRegistrationEntity
            {
                Code = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), 
                OperatorName = "operatorName", 
                Email = "email",
                PhoneNumber = 911111111,
                StoreAddress = "storeAddress",
                CompanyName = "companyName",
                StoreName = "storeName",
                CompanyLogoUrl = "companyLogoUrl",
                CityName = "cityName",
                Document = "document",
                IsApproved = true,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(preRegisterEntities);
        
        var preRegisterRepository = new PreRegistrationRepository(context);
        
        // Act
        var actualPreRegister = await preRegisterRepository.UpdatePreRegistrationById(Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3302"), false);
        
        // Assert
        actualPreRegister.Should().BeNull();
    }
    
    [Fact]
    public async Task DeletePreRegisterAsync_WhenPreRegisterExists_ReturnsPreRegisterCode()
    {
        // Arrange
        var preRegisterEntities = new List<PreRegistrationEntity>
        {
            new PreRegistrationEntity
            {
                Code = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), 
                OperatorName = "operatorName", 
                Email = "email",
                PhoneNumber = 911111111,
                StoreAddress = "storeAddress",
                CompanyName = "companyName",
                StoreName = "storeName",
                CompanyLogoUrl = "companyLogoUrl",
                CityName = "cityName",
                Document = "document",
                IsApproved = true,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(preRegisterEntities);
        
        var preRegisterRepository = new PreRegistrationRepository(context);
        
        // Act
        var actualPreRegisterCode = await preRegisterRepository.DeletePreRegisterAsync(Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"));
        
        // Assert
        context.PreRegister.Count().Should().Be(0);
    }
    
    [Fact]
    public async Task DeletePreRegisterAsync_WhenPreRegisterDoesNotExist_ReturnsNull()
    {
        // Arrange
        var preRegisterEntities = new List<PreRegistrationEntity>
        {
            new PreRegistrationEntity
            {
                Code = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), 
                OperatorName = "operatorName", 
                Email = "email",
                PhoneNumber = 911111111,
                StoreAddress = "storeAddress",
                CompanyName = "companyName",
                StoreName = "storeName",
                CompanyLogoUrl = "companyLogoUrl",
                CityName = "cityName",
                Document = "document",
                IsApproved = true,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(preRegisterEntities);
        
        var preRegisterRepository = new PreRegistrationRepository(context);
        
        // Act
        var actualPreRegisterCode = await preRegisterRepository.DeletePreRegisterAsync(Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3302"));
        
        // Assert
        actualPreRegisterCode.Should().BeNull();
    }
}