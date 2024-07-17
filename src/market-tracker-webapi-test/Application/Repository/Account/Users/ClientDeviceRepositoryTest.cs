using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Account;
using market_tracker_webapi.Application.Repository.Account.Users.Client;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Account;

namespace market_tracker_webapi_test.Application.Repository.Account.Users;

public class ClientDeviceRepositoryTest
{
    [Fact]
    public async Task GetDeviceTokensByClientIdAsync_ShouldReturnDeviceTokens()
    {
        // Arrange
        var fcmRegisterEntities = new List<FcmRegisterEntity>
        {
            new FcmRegisterEntity
            {
                ClientId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                DeviceId = "1",
                Token = "token1",
                UpdatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(fcmRegisterEntities);
        
        var clientDeviceRepository = new ClientDeviceRepository(context);
        
        // Act
        var actual = await clientDeviceRepository.GetDeviceTokensByClientIdAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        
        // Assert
        var expectedDeviceTokens = new List<DeviceToken>
        {
            new DeviceToken(Guid.Parse("00000000-0000-0000-0000-000000000001"), "1", "token1")
        };
        
        actual.Should().BeEquivalentTo(expectedDeviceTokens);
    }
    
    [Fact]
    public async Task GetDeviceTokenByDeviceIdAsync_ShouldReturnDeviceToken()
    {
        // Arrange
        var fcmRegisterEntities = new List<FcmRegisterEntity>
        {
            new FcmRegisterEntity
            {
                ClientId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                DeviceId = "1",
                Token = "token1",
                UpdatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(fcmRegisterEntities);
        
        var clientDeviceRepository = new ClientDeviceRepository(context);
        
        // Act
        var actual = await clientDeviceRepository.GetDeviceTokenByDeviceIdAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"), "1");
        
        // Assert
        var expectedDeviceToken = new DeviceToken(Guid.Parse("00000000-0000-0000-0000-000000000001"), "1", "token1");
        
        actual.Should().BeEquivalentTo(expectedDeviceToken);
    }
    
    [Fact]
    public async Task AddDeviceTokenAsync_ShouldReturnDeviceToken()
    {
        // Arrange
        var fcmRegisterEntities = new List<FcmRegisterEntity>();
        
        var context = DbHelper.CreateDatabase(fcmRegisterEntities);
        
        var clientDeviceRepository = new ClientDeviceRepository(context);
        
        // Act
        var actual = await clientDeviceRepository.AddDeviceTokenAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"), "1", "token1");
        
        // Assert
        var expectedDeviceToken = new DeviceToken(Guid.Parse("00000000-0000-0000-0000-000000000001"), "1", "token1");
        
        actual.Should().BeEquivalentTo(expectedDeviceToken);
    }
    
    [Fact]
    public async Task UpdateDeviceTokenAsync_ShouldReturnDeviceToken()
    {
        // Arrange
        var fcmRegisterEntities = new List<FcmRegisterEntity>
        {
            new FcmRegisterEntity
            {
                ClientId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                DeviceId = "1",
                Token = "token1",
                UpdatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(fcmRegisterEntities);
        
        var clientDeviceRepository = new ClientDeviceRepository(context);
        
        // Act
        var actual = await clientDeviceRepository.UpdateDeviceTokenAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"), "1", "token2");
        
        // Assert
        var expectedDeviceToken = new DeviceToken(Guid.Parse("00000000-0000-0000-0000-000000000001"), "1", "token2");
        
        actual.Should().BeEquivalentTo(expectedDeviceToken);
    }
    
    [Fact]
    public async Task RemoveDeviceTokenAsync_ShouldReturnDeviceToken()
    {
        // Arrange
        var fcmRegisterEntities = new List<FcmRegisterEntity>
        {
            new FcmRegisterEntity
            {
                ClientId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                DeviceId = "1",
                Token = "token1",
                UpdatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(fcmRegisterEntities);
        
        var clientDeviceRepository = new ClientDeviceRepository(context);
        
        // Act
        var actual = await clientDeviceRepository.RemoveDeviceTokenAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"), "1");
        
        // Assert
        var expectedDeviceToken = new DeviceToken(Guid.Parse("00000000-0000-0000-0000-000000000001"), "1", "token1");
        
        actual.Should().BeEquivalentTo(expectedDeviceToken);
    }
}