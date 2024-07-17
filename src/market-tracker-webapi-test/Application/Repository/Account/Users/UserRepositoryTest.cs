using FluentAssertions;
using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Repository.Account.Users.User;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Account.Users;

namespace market_tracker_webapi_test.Application.Repository.Account.Users;

public class UserRepositoryTest
{
    [Fact]
    public async Task GetUsersAsync_ShouldReturnPaginatedResultOfUserItem()
    {
        // Arrange
        var userEntities = new List<UserEntity>()
        {
            new UserEntity
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Role = Role.Client.ToString(),
                Name = "user1",
                Email = "user1@gmail.com",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(userEntities);
        
        var userRepository = new UserRepository(context);
        
        // Act
        var actual = await userRepository.GetUsersAsync(Role.Client.ToString(), 0, 10);
        
        // Assert
        var expectedPaginatedUsers = new PaginatedResult<UserItem>(
            new List<UserItem>
            {
                new UserItem(Guid.Parse("00000000-0000-0000-0000-000000000001"), "user1", Role.Client.ToString())
            }
            , 1, 0, 10);
        
        actual.Should().BeEquivalentTo(expectedPaginatedUsers);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser()
    {
        // Arrange
        var userEntities = new List<UserEntity>()
        {
            new UserEntity
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Role = Role.Client.ToString(),
                Name = "user1",
                Email = "user1@gmail.com",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(userEntities);
        
        var userRepository = new UserRepository(context);
        
        // Act
        var actual = await userRepository.GetUserByIdAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        
        // Assert
        var expectedUser = new User(
            Guid.Parse("00000000-0000-0000-0000-000000000001"), 
            "user1", "user1@gmail.com", 
            Role.Client.ToString(), 
            new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            );
        
        actual.Should().BeEquivalentTo(expectedUser);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnUser()
    {
        var userEntities = new List<UserEntity>()
        {
            new UserEntity
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Role = Role.Client.ToString(),
                Name = "user1",
                Email = "user1@gmail.com",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(userEntities);
        
        var userRepository = new UserRepository(context);
        
        // Act
        var actual = await userRepository.GetUserByEmailAsync("user1@gmail.com");
        
        // Assert
        var expectedUser = new User(
            Guid.Parse("00000000-0000-0000-0000-000000000001"), 
            "user1", "user1@gmail.com", 
            Role.Client.ToString(), 
            new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
        );
        
        actual.Should().BeEquivalentTo(expectedUser);
    }
    
    [Fact]
    public async Task GetUserByUsernameAsync_ShouldReturnUser()
    {
        var userEntities = new List<UserEntity>()
        {
            new UserEntity
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Role = Role.Client.ToString(),
                Name = "user1",
                Email = "user1@gmail.com",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(userEntities);
        
        var userRepository = new UserRepository(context);
        
        // Act
        var actual = await userRepository.GetUserByUsernameAsync("user1");
        
        // Assert
        var expectedUser = new User(
            Guid.Parse("00000000-0000-0000-0000-000000000001"), 
            "user1", "user1@gmail.com", 
            Role.Client.ToString(), 
            new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
        );
        
        actual.Should().BeEquivalentTo(expectedUser);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnUserId()
    {
        var userEntities = new List<UserEntity>();

        var context = DbHelper.CreateDatabase(userEntities);

        var userRepository = new UserRepository(context);

        // Act
        var actual = await userRepository.CreateUserAsync("user1", "user1@gmail.com", Role.Client.ToString());
        
        // Assert
        var expectedUserEntity = new UserEntity
        {
            Name = "user1",
            Email = "user1@gmail.com",
            Role = Role.Client.ToString(),
            CreatedAt = DateTime.UtcNow,
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001")
        };
        
        context.User.Should().ContainEquivalentOf(expectedUserEntity, x => x
            .Excluding(y => y.Id)
            .Excluding(y => y.CreatedAt)
        );
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnUser()
    {
        var userEntities = new List<UserEntity>()
        {
            new UserEntity
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Role = Role.Client.ToString(),
                Name = "user1",
                Email = "user1@gmail.com",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(userEntities);
        
        var userRepository = new UserRepository(context);
        
        // Act
        var actual = await userRepository.UpdateUserAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"), "newUser");
        
        // Assert
        var expectedUser = new User(
            Guid.Parse("00000000-0000-0000-0000-000000000001"), 
            "newUser", "user1@gmail.com", 
            Role.Client.ToString(), 
            new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
        );

        actual.Should().BeEquivalentTo(expectedUser, x => x
            .Excluding(y => y.Id)
            .Excluding(y => y.CreatedAt)
        );
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldReturnDeletedUser()
    {
        var userEntities = new List<UserEntity>()
        {
            new UserEntity
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Role = Role.Client.ToString(),
                Name = "user1",
                Email = "user1@gmail.com",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(userEntities);
        
        var userRepository = new UserRepository(context);
        
        // Act
        var actual = await userRepository.DeleteUserAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        
        // Assert
        var expectedUser = new User(
            Guid.Parse("00000000-0000-0000-0000-000000000001"), 
            "user1", "user1@gmail.com", 
            Role.Client.ToString(), 
            new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
        );

        actual.Should().BeEquivalentTo(expectedUser, x => x
            .Excluding(y => y.Id)
            .Excluding(y => y.CreatedAt)
        );

        context.User.Should().BeEmpty();
    }

}