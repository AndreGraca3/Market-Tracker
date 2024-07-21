using FluentAssertions;
using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Http.Controllers.Account.Users;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Client;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Service.Operations.Account.Users.Client;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace market_tracker_webapi_test.Application.Controllers;

public class ClientControllerTest
{
    private readonly Mock<IClientService> _clientService;
    private readonly Mock<IClientDeviceService> _clientDeviceService;
    
    private readonly ClientController _clientController;
    
    public ClientControllerTest()
    {
        _clientService = new Mock<IClientService>();
        _clientDeviceService = new Mock<IClientDeviceService>();
        
        _clientController = new ClientController(_clientService.Object, _clientDeviceService.Object);
    }
    
    [Fact]
    public async Task GetClientsAsync_ReturnsPaginatedResult()
    {
        // Arrange
        var paginationInputs = new PaginationInputs()
        {
            ItemsPerPage = 1,
            Page = 1
        };
        
        var paginatedClients = new PaginatedResult<ClientItem>(
            new List<ClientItem>()
            {
                new(
                    Guid.Parse("f1b9b3b4-0b3b-4b3b-8b3b-0b3b3b3b3b3b"),
                    "username",
                    "avatarUrl"
                    )
            },
            1, 
            1,
            1
            );
        
        _clientService
            .Setup(x => x.GetClientsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(paginatedClients);
        
        // Act
        var result = await _clientController.GetClientsAsync(paginationInputs, It.IsAny<string>());
        
        // Assert
        result.Should().BeEquivalentTo(paginatedClients.Select(c => c.ToOutputModel()), x => x.ExcludingMissingMembers());
    }
    
    [Fact]
    public async Task GetClientAsync_ReturnsClientOutputModel()
    {
        // Arrange
        var client = new Client(
            new User(
                Guid.Parse("f1b9b3b4-0b3b-4b3b-8b3b-0b3b3b3b3b3b"),
                "username",
                "email",
                Role.Client.ToString(),
                new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
                ),
            "username",
            "avatarUrl"
            );
        
        _clientService
            .Setup(x => x.GetClientByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(client);
        
        // Act
        var result = await _clientController.GetClientAsync(It.IsAny<Guid>());
        
        // Assert
        result.Should().BeEquivalentTo(client.ToOutputModel(), x => x.ExcludingMissingMembers());
    }
    
    // [Fact]
    // public async Task GetAuthenticatedClientAsync_ReturnsAuthClientOutputModel()
    // {
    //     // Arrange
    //     var client = new Client(
    //         new User(
    //             Guid.Parse("f1b9b3b4-0b3b-4b3b-8b3b-0b3b3b3b3b3b"),
    //             "username",
    //             "email",
    //             Role.Client.ToString(),
    //             new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
    //         ),
    //         "username",
    //         "avatarUrl"
    //     );
    //     
    //     _clientService
    //         .Setup(x => x.GetClientByIdAsync(It.IsAny<Guid>()))
    //         .ReturnsAsync(client);
    //     
    //     // Act
    //     var result = await _clientController.GetAuthenticatedClientAsync();
    //     
    //     // Assert
    //     result.Should().BeEquivalentTo(client.ToAuthOutputModel(), x => x.ExcludingMissingMembers());
    // }
    
    [Fact]
    public async Task CreateClientAsync_ReturnsUserId()
    {
        // Arrange
        var userId = new UserId(Guid.Parse("f1b9b3b4-0b3b-4b3b-8b3b-0b3b3b3b3b3b"));
        var newClient = new ClientCreationInputModel(
            "username",
            "name",
            "email",
            "password",
            "avatar"
            );
        
        _clientService
            .Setup(x => x.CreateClientAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(userId);
        
        // Act
        var result = await _clientController.CreateClientAsync(newClient);
        
        // Assert
        result.Should().BeOfType<ActionResult<UserId>>();
    }
    
    // [Fact]
    // public async Task UpdateUserAsync_ReturnsClientOutputModel()
    // {
    //     // Arrange
    //     var client = new Client(
    //         new User(
    //             Guid.Parse("f1b9b3b4-0b3b-4b3b-8b3b-0b3b3b3b3b3b"),
    //             "username",
    //             "email",
    //             Role.Client.ToString(),
    //             new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
    //             ),
    //         "username",
    //         "avatarUrl"
    //         );
    //     
    //     var clientUpdateInputModel = new ClientUpdateInputModel(
    //         "name",
    //         "username",
    //         "avatar"
    //         );
    //     
    //     _clientService
    //         .Setup(x => x.UpdateClientAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
    //         .ReturnsAsync(client);
    //     
    //     // Act
    //     var result = await _clientController.UpdateUserAsync(clientUpdateInputModel);
    //     
    //     // Assert
    //     result.Should().BeEquivalentTo(client.ToOutputModel(), x => x.ExcludingMissingMembers());
    // }
    
}