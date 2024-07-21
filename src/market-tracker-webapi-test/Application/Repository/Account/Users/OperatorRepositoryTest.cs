using FluentAssertions;
using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Repository.Account.Users.Operator;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Account.Users;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Store;

namespace market_tracker_webapi_test.Application.Repository.Account.Users;

public class OperatorRepositoryTest
{
    [Fact]
    public async Task GetOperatorsAsync_ShouldReturnPaginatedResultOfOperatorItem()
    {
        // Arrange
        var userEntities = new List<UserEntity>()
        {
            new UserEntity
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Role = Role.Operator.ToString(),
                Name = "user1",
                Email = "user1@gmail.com",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var operatorEntities = new List<OperatorEntity>()
        {
            new()
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                PhoneNumber = 1234567890
            }
        };
        
        var storeEntities = new List<StoreEntity>()
        {
            new()
            {
                Id = 1,
                OperatorId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                CompanyId = 1,
                Address = "address1",
                Name = "store1",
                CityId = 1
            }
        };
        
        var companyEntities = new List<CompanyEntity>()
        {
            new()
            {
                Id = 1,
                Name = "company1",
                LogoUrl = "logo1",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(userEntities, operatorEntities, storeEntities, companyEntities);
        
        var operatorRepository = new OperatorRepository(context);
        
        // Act
        var actual = await operatorRepository.GetOperatorsAsync(0, 10);
        
        // Assert
        var expectedPaginatedOperators = new PaginatedResult<OperatorItem>(
            new List<OperatorItem>
            {
                new OperatorItem(Guid.Parse("00000000-0000-0000-0000-000000000001"), "user1", "logo1")
            },
            1,
            0,
            10
        );
        
        actual.Should().BeEquivalentTo(expectedPaginatedOperators);
    }

    [Fact]
    public async Task GetOperatorByIdAsync_ShouldReturnOperator()
    {
        // Arrange
        var userEntities = new List<UserEntity>()
        {
            new UserEntity
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Role = Role.Operator.ToString(),
                Name = "user1",
                Email = "user1@gmail.com",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };

        var operatorEntities = new List<OperatorEntity>()
        {
            new()
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                PhoneNumber = 1234567890
            }
        };

        var storeEntities = new List<StoreEntity>()
        {
            new()
            {
                Id = 1,
                OperatorId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                CompanyId = 1,
                Address = "address1",
                Name = "store1",
                CityId = 1
            }
        };

        var companyEntities = new List<CompanyEntity>()
        {
            new()
            {
                Id = 1,
                Name = "company1",
                LogoUrl = "logo1",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };

        var context = DbHelper.CreateDatabase(userEntities, operatorEntities, storeEntities, companyEntities);

        var operatorRepository = new OperatorRepository(context);
        
        // Act
        var actual = await operatorRepository.GetOperatorByIdAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        
        // Assert
        var expectedOperator = new Operator(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "user1",
            "user1@gmail.com",
            1234567890,
            new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified));
        
        actual.Should().BeEquivalentTo(expectedOperator);
    }

    [Fact]
    public async Task GetOperatorByEmailAsync_ShouldReturnOperator()
    {
        // Arrange
        var userEntities = new List<UserEntity>()
        {
            new UserEntity
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Role = Role.Operator.ToString(),
                Name = "user1",
                Email = "user1@gmail.com",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var operatorEntities = new List<OperatorEntity>()
        {
            new()
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                PhoneNumber = 1234567890
            }
        };
        
        var storeEntities = new List<StoreEntity>()
        {
            new()
            {
                Id = 1,
                OperatorId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                CompanyId = 1,
                Address = "address1",
                Name = "store1",
                CityId = 1
            }
        };
        
        var companyEntities = new List<CompanyEntity>()
        {
            new()
            {
                Id = 1,
                Name = "company1",
                LogoUrl = "logo1",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(userEntities, operatorEntities, storeEntities, companyEntities);
        
        var operatorRepository = new OperatorRepository(context);
        
        // Act
        var actual = await operatorRepository.GetOperatorByEmailAsync("user1@gmail.com");
        
        // Assert
        var expectedOperator = new Operator(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "user1",
            "user1@gmail.com",
            1234567890,
            new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified));
        
        actual.Should().BeEquivalentTo(expectedOperator);
    }

    [Fact]
    public async Task CreateOperatorAsync_ShouldReturnUserId()
    {
        var context = DbHelper.CreateDatabase();

        var operatorRepository = new OperatorRepository(context);

        var actual =
            await operatorRepository.CreateOperatorAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"),
                1234567890);

        var expected = new UserId(Guid.Parse("00000000-0000-0000-0000-000000000001"));

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task UpdateOperatorAsync_ShouldReturnOperator()
    {
        // Arrange
        var userEntities = new List<UserEntity>()
        {
            new UserEntity
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Role = Role.Operator.ToString(),
                Name = "user1",
                Email = "user1@gmail.com",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };

        var operatorEntities = new List<OperatorEntity>()
        {
            new()
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                PhoneNumber = 1234567890
            }
        };

        var storeEntities = new List<StoreEntity>()
        {
            new()
            {
                Id = 1,
                OperatorId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                CompanyId = 1,
                Address = "address1",
                Name = "store1",
                CityId = 1
            }
        };

        var companyEntities = new List<CompanyEntity>()
        {
            new()
            {
                Id = 1,
                Name = "company1",
                LogoUrl = "logo1",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };

        var context = DbHelper.CreateDatabase(userEntities, operatorEntities, storeEntities, companyEntities);

        var operatorRepository = new OperatorRepository(context);
        
        // Act
        var actual = await operatorRepository.UpdateOperatorAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"), 999999999);
        
        // Assert
        var expectedOperator = new Operator(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "user1",
            "user1@gmail.com",
            999999999,
            new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified));
        
        actual.Should().BeEquivalentTo(expectedOperator);
    }
    
    [Fact]
    public async Task DeleteOperatorAsync_ShouldReturnNull()
    {
        // Arrange
        var context = DbHelper.CreateDatabase();
        
        var operatorRepository = new OperatorRepository(context);
        
        // Act
        var actual = await operatorRepository.DeleteOperatorAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        
        // Assert
        actual.Should().BeNull();
    }
}