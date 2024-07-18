using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Account;
using market_tracker_webapi.Application.Repository.Account.Users.Client;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Operations.Account.Users.Client;
using Moq;

namespace market_tracker_webapi_test.Application.Service;

public class ClientDeviceServiceTest
{
    private readonly Mock<IClientDeviceRepository> _clientDeviceRepositoryMock;
    
    private readonly IClientDeviceService _clientService;
    
    public ClientDeviceServiceTest()
    {
        _clientDeviceRepositoryMock = new Mock<IClientDeviceRepository>();
        
        _clientService = new ClientDeviceService(
            _clientDeviceRepositoryMock.Object,
            new MockedTransactionManager()
            );
    }
    
    [Fact]
    public async Task UpsertNotificationDeviceAsync_ShouldReturnDeviceToken_WhenDeviceTokenIsNotPresent()
    {
        // Arrange
        var deviceToken = new DeviceToken(
                Guid.Parse("00000000-0000-0000-0000-000000000001"),
                "Device 1",
                "Token 1"
            );
        
        var updatedDeviceToken = new DeviceToken(
                Guid.Parse("00000000-0000-0000-0000-000000000001"),
                "Device 1",
                "Token 2"
                );
        
        _clientDeviceRepositoryMock
            .Setup(x => x.GetDeviceTokenByDeviceIdAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync(deviceToken);

        _clientDeviceRepositoryMock
            .Setup(x => x.UpdateDeviceTokenAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(updatedDeviceToken);
        
        // Act
        var result = await _clientService.UpsertNotificationDeviceAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "Device 1",
            "randomToken"
            );
        
        // Assert
        result.Should().BeEquivalentTo(updatedDeviceToken);
    }
    
    [Fact]
    public async Task UpsertNotificationDeviceAsync_ShouldReturnDeviceToken_WhenDeviceTokenIsPresent()
    {
        // Arrange
        var deviceToken = new DeviceToken(
                Guid.Parse("00000000-0000-0000-0000-000000000001"),
                "Device 1",
                "Token 1"
            );
        
        _clientDeviceRepositoryMock
            .Setup(x => x.GetDeviceTokenByDeviceIdAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync((DeviceToken)null);

        _clientDeviceRepositoryMock
            .Setup(x => x.AddDeviceTokenAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(deviceToken);
        
        // Act
        var result = await _clientService.UpsertNotificationDeviceAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "Device 1",
            "randomToken"
            );
        
        // Assert
        result.Should().BeEquivalentTo(deviceToken);
    }
    
    [Fact]
    public async Task DeRegisterNotificationDeviceAsync_ShouldReturnDeviceToken_WhenDeviceTokenIsPresent()
    {
        // Arrange
        var deviceToken = new DeviceToken(
                Guid.Parse("00000000-0000-0000-0000-000000000001"),
                "Device 1",
                "Token 1"
            );
        
        _clientDeviceRepositoryMock
            .Setup(x => x.RemoveDeviceTokenAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync(deviceToken);
        
        // Act
        var result = await _clientService.DeRegisterNotificationDeviceAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "Device 1"
            );
        
        // Assert
        result.Should().BeEquivalentTo(deviceToken);
    }
}