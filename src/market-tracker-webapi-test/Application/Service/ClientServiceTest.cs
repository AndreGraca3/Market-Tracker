using FluentAssertions;
using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Repository.Account.Credential;
using market_tracker_webapi.Application.Repository.Account.Users.Client;
using market_tracker_webapi.Application.Repository.Account.Users.User;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Operations.Account.Users.Client;
using Moq;

namespace market_tracker_webapi_test.Application.Service;

public class ClientServiceTest
{
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    private readonly ClientService _clientService;

    public ClientServiceTest()
    {
        _clientRepositoryMock = new Mock<IClientRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        Mock<IAccountRepository> accountRepositoryMock = new();

        _clientService = new ClientService(accountRepositoryMock.Object, _clientRepositoryMock.Object,
            _userRepositoryMock.Object, new MockedTransactionManager());
    }

    [Fact]
    public async Task GetAllClientsAsync_ShouldReturnAllClientsPaginated()
    {
        // Arrange
        const int skip = 0;
        const int take = 10;

        var paginatedResult = new PaginatedResult<ClientItem>(
            MockedData.DummyClients.Select(c => c.ToClientItem()),
            MockedData.DummyClients.Count, skip, take);

        _clientRepositoryMock
            .Setup(x => x.GetClientsAsync(It.IsAny<string>(), skip, take))
            .ReturnsAsync(paginatedResult);

        // Act
        var result = await _clientService.GetClientsAsync(null, skip, take);

        // Assert
        result.Should().BeEquivalentTo(paginatedResult);
    }

    [Fact]
    public async Task GetClientsWithUsernameAsync_ShouldReturnClientsPaginated()
    {
        // Arrange
        var username = MockedData.DummyClients.First().Username;
        var skip = 0;
        var take = 10;

        var paginatedResult = new PaginatedResult<ClientItem>(
            MockedData.DummyClients.Select(c => c.ToClientItem()),
            MockedData.DummyClients.Count, skip, take);

        _clientRepositoryMock
            .Setup(x => x.GetClientsAsync(username, skip, take))
            .ReturnsAsync(paginatedResult);

        // Act
        var result = await _clientService.GetClientsAsync(username, skip, take);

        // Assert
        result.Should().BeEquivalentTo(paginatedResult);
    }

    [Fact]
    public async Task GetClientByIdAsync_ShouldReturnClient()
    {
        // Arrange
        var clientId = MockedData.DummyClients.First().Id.Value;

        _clientRepositoryMock
            .Setup(x => x.GetClientByIdAsync(clientId))
            .ReturnsAsync(MockedData.DummyClients.First());

        // Act
        var result = await _clientService.GetClientByIdAsync(clientId);

        // Assert
        result.Should().BeEquivalentTo(MockedData.DummyClients.First());
    }

    [Fact]
    public async Task GetClientByIdAsync_ShouldReturnClientByIdNotFound()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        _clientRepositoryMock
            .Setup(x => x.GetClientByIdAsync(clientId))
            .ReturnsAsync((Client?)null);

        // Act
        var ex = await Assert.ThrowsAsync<MarketTrackerServiceException>(() =>
            _clientService.GetClientByIdAsync(clientId));

        // Assert
        ex.ServiceError
            .Should()
            .BeEquivalentTo(new UserFetchingError.UserByIdNotFound(clientId));
    }

    [Fact]
    public async Task CreateClientAsync_ShouldCreateClient()
    {
        // Arrange
        const string username = "username";
        const string name = "name";
        const string email = "email";
        const string password = "password";
        const string avatarUrl = "avatarUrl";

        _userRepositoryMock
            .Setup(x => x.GetUserByEmailAsync(email))
            .ReturnsAsync((User?)null);

        _clientRepositoryMock
            .Setup(x => x.GetClientByUsernameAsync(username))
            .ReturnsAsync((Client?)null);

        _userRepositoryMock
            .Setup(x => x.CreateUserAsync(name, email, Role.Client.ToString()))
            .ReturnsAsync(new UserId(Guid.NewGuid()));

        // Act
        var result = await _clientService.CreateClientAsync(username, name, email, password, avatarUrl);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateClientWithInvalidUsernameAsync_ShouldThrowCredentialAlreadyInUse()
    {
        // Arrange
        const string username = "username";
        const string name = "name";
        const string email = "email";
        const string password = "password";
        const string avatarUrl = "avatarUrl";

        _clientRepositoryMock
            .Setup(x => x.GetClientByUsernameAsync(username))
            .ReturnsAsync(new Client(new User(Guid.NewGuid(), name, email, Role.Client.ToString(), DateTime.UtcNow),
                username, avatarUrl));

        // Act
        var ex = await Assert.ThrowsAsync<MarketTrackerServiceException>(() =>
            _clientService.CreateClientAsync(username, name, email, password, avatarUrl));

        // Assert
        ex.ServiceError
            .Should()
            .BeEquivalentTo(new UserCreationError.CredentialAlreadyInUse(username, nameof(username)));
    }
    
    [Fact]
    public async Task CreateClientAsync_ShouldThrowCredentialAlreadyInUse()
    {
        // Arrange
        const string username = "username";
        const string name = "name";
        const string email = "email";
        const string password = "password";
        const string avatarUrl = "avatarUrl";

        _userRepositoryMock
            .Setup(x => x.GetUserByEmailAsync(email))
            .ReturnsAsync(new User(Guid.NewGuid(), name, email, Role.Client.ToString(), DateTime.UtcNow));

        // Act
        var ex = await Assert.ThrowsAsync<MarketTrackerServiceException>(() =>
            _clientService.CreateClientAsync(username, name, email, password, avatarUrl));

        // Assert
        ex.ServiceError
            .Should()
            .BeEquivalentTo(new UserCreationError.CredentialAlreadyInUse(email, nameof(email)));
    }

    [Fact]
    public async Task UpdateClientAsync_ShouldUpdateClient()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        const string name = "name";
        const string username = "username";
        const string avatarUrl = "avatarUrl";

        _userRepositoryMock
            .Setup(x => x.GetUserByUsernameAsync(name))
            .ReturnsAsync((User?)null);

        var user = new User(clientId, name, "email", Role.Client.ToString(), DateTime.UtcNow);
        _userRepositoryMock
            .Setup(x => x.UpdateUserAsync(clientId, name))
            .ReturnsAsync(user);

        _clientRepositoryMock
            .Setup(x => x.UpdateClientAsync(clientId, username, avatarUrl))
            .ReturnsAsync(new Client(user, username, avatarUrl).ToClientItem());

        // Act
        var result = await _clientService.UpdateClientAsync(clientId, name, username, avatarUrl);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateClientAsync_ShouldThrowInvalidUsername()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        const string name = "name";
        const string username = "username";
        const string avatarUrl = "avatarUrl";

        _userRepositoryMock
            .Setup(x => x.GetUserByUsernameAsync(name))
            .ReturnsAsync(new User(clientId, name, "email", Role.Client.ToString(), DateTime.UtcNow));

        // Act
        var ex = await Assert.ThrowsAsync<MarketTrackerServiceException>(() =>
            _clientService.UpdateClientAsync(clientId, name, username, avatarUrl));

        // Assert
        ex.ServiceError
            .Should()
            .BeEquivalentTo(new UserCreationError.InvalidUsername(name), x => x.ExcludingMissingMembers());
    }

    [Fact]
    public async Task UpdateClientAsync_ShouldThrowUserByIdNotFound()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        const string name = "name";
        const string username = "username";
        const string avatarUrl = "avatarUrl";

        _userRepositoryMock
            .Setup(x => x.GetUserByUsernameAsync(name))
            .ReturnsAsync((User?)null);

        _userRepositoryMock
            .Setup(x => x.UpdateUserAsync(clientId, name))
            .ReturnsAsync((User?)null);

        // Act
        var ex = await Assert.ThrowsAsync<MarketTrackerServiceException>(() =>
            _clientService.UpdateClientAsync(clientId, name, username, avatarUrl));

        // Assert
        ex.ServiceError
            .Should()
            .BeEquivalentTo(new UserFetchingError.UserByIdNotFound(clientId));
    }

    [Fact]
    public async Task DeleteClientAsync_ShouldDeleteClient()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        _userRepositoryMock
            .Setup(x => x.DeleteUserAsync(clientId))
            .ReturnsAsync(new User(clientId, "name", "email", Role.Client.ToString(), DateTime.UtcNow));

        // Act
        await _clientService.DeleteClientAsync(clientId);
    }

    [Fact]
    public async Task DeleteClientAsync_ShouldThrowUserByIdNotFound()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        _userRepositoryMock
            .Setup(x => x.DeleteUserAsync(clientId))
            .ReturnsAsync((User?)null);

        // Act
        var ex = await Assert.ThrowsAsync<MarketTrackerServiceException>(() =>
            _clientService.DeleteClientAsync(clientId));

        // Assert
        ex.ServiceError
            .Should()
            .BeEquivalentTo(new UserFetchingError.UserByIdNotFound(clientId));
    }
}