using FluentAssertions;
using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Repository.Account.Credential;
using market_tracker_webapi.Application.Repository.Account.Token;
using market_tracker_webapi.Application.Repository.Account.Users.User;
using market_tracker_webapi.Application.Service.Operations.Account.Users.User;
using Moq;

namespace market_tracker_webapi_test.Application.Service;

public class UserServiceTest
{
    private readonly Mock<IUserRepository> userRepositoryMock;
    private readonly Mock<ITokenRepository> tokenRepositoryMock;
    private readonly Mock<IAccountRepository> accountRepositoryMock;

    private readonly IUserService _userService;

    public UserServiceTest()
    {
        userRepositoryMock = new Mock<IUserRepository>();
        tokenRepositoryMock = new Mock<ITokenRepository>();
        accountRepositoryMock = new Mock<IAccountRepository>();

        _userService = new UserService(
            userRepositoryMock.Object,
            tokenRepositoryMock.Object,
            accountRepositoryMock.Object,
            new MockedTransactionManager()
        );
    }

    [Fact]
    public async Task GetUsersAsync_ShouldReturnPaginatedResult()
    {
        // Arrange
        var userItems = new List<UserItem>
        {
            new UserItem(
                Guid.Parse("00000000-0000-0000-0000-000000000001"), 
                "user1", 
                Role.Client.ToString())
        };
        
        var paginatedResult = new PaginatedResult<UserItem>(userItems, 1, 1, 10);
        
        userRepositoryMock
            .Setup(x => x.GetUsersAsync(Role.Client.ToString(), 0, 10))
            .ReturnsAsync(paginatedResult);
        
        // Act
        var result = await _userService.GetUsersAsync(Role.Client.ToString(), 0, 10);
        
        // Assert
        result.Should().BeEquivalentTo(paginatedResult);
    }
    
    [Fact]
    public async Task GetUserAsync_ShouldReturnUser()
    {
        // Arrange
        var user = new User(
            Guid.Parse("00000000-0000-0000-0000-000000000001"), 
            "user1", 
            "user1@gmail.com",
            Role.Client.ToString(),
            new DateTime(2024, 01, 01, 0 ,0 ,0, DateTimeKind.Unspecified));
        
        userRepositoryMock
            .Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(user);
        
        // Act
        var result = await _userService.GetUserAsync(It.IsAny<Guid>());
        
        // Assert
        result.Should().BeEquivalentTo(user);
    }
}