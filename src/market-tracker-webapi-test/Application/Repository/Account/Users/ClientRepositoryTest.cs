using FluentAssertions;
using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Repository.Account.Users.Client;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Account.Users;

namespace market_tracker_webapi_test.Application.Repository.Account.Users;

public class ClientRepositoryTest
{
    [Fact]
    public async Task GetClientsAsync_ShouldReturnPaginatedResultOfClientItem()
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
                CreatedAt = new DateTime(2024,01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };

        var clientEntities = new List<ClientEntity>()
        {
            new()
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Username = "user1",
                Avatar = null
            }
        };

        var context = DbHelper.CreateDatabase(userEntities, clientEntities);
        
        var clientRepository = new ClientRepository(context);
        
        // Act
        var actual = await clientRepository.GetClientsAsync("user1", 0, 10);

        // Assert
        var expectedPaginatedClients = new PaginatedResult<ClientItem>(
            new List<ClientItem>
            {
                new ClientItem(Guid.Parse("00000000-0000-0000-0000-000000000001"), "user1", null)
            },
            1,
            0,
            10
        );
        
        actual.Should().BeEquivalentTo(expectedPaginatedClients);
    }

    [Fact]
    public async Task GetClientByIdAsync_ShouldReturnClient()
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

        var clientEntities = new List<ClientEntity>()
        {
            new()
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Username = "user1",
                Avatar = null
            }
        };
        
        var context = DbHelper.CreateDatabase(userEntities, clientEntities);
        
        var clientRepository = new ClientRepository(context);
        
        // Act
        var actual = await clientRepository.GetClientByIdAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        
        // Assert
        var expectedClient = new Client(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "user1",
            "user1",
            "user1@gmail.com",
            new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
            null);
        
        actual.Should().BeEquivalentTo(expectedClient);
    }

    [Fact]
    public async Task GetClientByUsernameAsync_ShouldReturnClient()
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

        var clientEntities = new List<ClientEntity>()
        {
            new()
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Username = "user1",
                Avatar = null
            }
        };
        
        var context = DbHelper.CreateDatabase(userEntities, clientEntities);
        
        var clientRepository = new ClientRepository(context);
        
        // Act
        var actual = await clientRepository.GetClientByUsernameAsync("user1");
        
        // Assert
        var expectedClient = new Client(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "user1",
            "user1",
            "user1@gmail.com",
            new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
            null);
        
        actual.Should().BeEquivalentTo(expectedClient);
    }

    [Fact]
    public async Task CreateClientAsync_ShouldReturnUserId()
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

        var clientEntities = new List<ClientEntity>()
        {
            new()
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Username = "user1",
                Avatar = null
            }
        };

        var context = DbHelper.CreateDatabase(userEntities, clientEntities);

        var clientRepository = new ClientRepository(context);

        // Act
        var actual = await clientRepository.CreateClientAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000002"),
            "user2",
            null
        );
        
        // Assert
        var expectedUserId = new UserId(Guid.Parse("00000000-0000-0000-0000-000000000002"));
        
        actual.Should().BeEquivalentTo(expectedUserId);
    }

    [Fact]
    public async Task UpdateClientAsync_ShouldReturnClientItem()
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

        var clientEntities = new List<ClientEntity>()
        {
            new()
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Username = "user1",
                Avatar = null
            }
        };

        var context = DbHelper.CreateDatabase(userEntities, clientEntities);

        var clientRepository = new ClientRepository(context);
        
        // Act
        var actual = await clientRepository.UpdateClientAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "user2",
            null
        );
        
        // Assert
        var expectedClientItem = new ClientItem(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "user2",
            null
        );
        
        actual.Should().BeEquivalentTo(expectedClientItem);
    }

    [Fact]
    public async Task DeleteClientAsync_ShouldReturnClient()
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

        var clientEntities = new List<ClientEntity>()
        {
            new()
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Username = "user1",
                Avatar = null
            }
        };

        var context = DbHelper.CreateDatabase(userEntities, clientEntities);

        var clientRepository = new ClientRepository(context);
        
        // Act
        var actual = await clientRepository.DeleteClientAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        
        // Assert
        var expectedClient = new ClientItem(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            null,
            null
        );
        
        actual.Should().BeEquivalentTo(expectedClient);
    }
}