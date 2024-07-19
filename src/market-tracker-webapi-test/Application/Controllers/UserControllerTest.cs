using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Http.Controllers.Account.Users;
using market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.User;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Service.Operations.Account.Users.User;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace market_tracker_webapi_test.Application.Controllers;

public class UserControllerTest
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly UserController _userController;

    public UserControllerTest()
    {
        _userServiceMock = new Mock<IUserService>();
        _userController = new UserController(_userServiceMock.Object);
    }
    
    [Fact]
    public async Task GetUser_ReturnsOk()
    {
        // Arrange
        var user = new User(
            Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), 
            "User 1", 
            "user1@gmail.com", 
            Role.Client.ToString(),
            new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified));
        
        _userServiceMock
            .Setup(x => x.GetUserAsync(It.IsAny<Guid>()))
            .ReturnsAsync(user);

        // Act
        var result = await _userController.GetUserAsync(It.IsAny<Guid>());

        // Assert
        result.Value.Should().BeEquivalentTo(user.ToOutputModel());
    }

    // [Fact]
    // public async Task AddUser_ReturnsCreated()
    // {
    //     // Arrange
    //     var user = new User(
    //         Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), 
    //         "User 1", 
    //         "user1@gmail.com", 
    //         Role.Client.ToString(),
    //         new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified));
    //     
    //     _userServiceMock
    //         .Setup(x => x.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
    //         .ReturnsAsync(new UserId(Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")));
    //         
    //     // Act
    //     var result = await _userController.CreateUserAsync(new UserCreationInputModel(
    //         "User 1",
    //         "user1@gmail.com",
    //         "password",
    //         Role.Client.ToString()
    //         ));
    //     
    //     // Assert
    //     result.Value.Should().BeEquivalentTo(new UserCreationOutputModel(
    //         Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
    //         ));
    // }

    // [Fact]
    // public async Task UpdateUserAsync_ReturnsOk()
    // {
    //     // Arrange
    //     var user = new User(
    //         Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), 
    //         "User 1", 
    //         "user1@gmail.com", 
    //         Role.Client.ToString(),
    //         new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified));
    //
    //     _userServiceMock
    //         .Setup(x => x.UpdateUserAsync(It.IsAny<Guid>(), It.IsAny<string>()))
    //         .ReturnsAsync(user);
    //
    //     // Act
    //     var result = await _userController.UpdateUserAsync(It.IsAny<Guid>(), It.IsAny<UserUpdateInputModel>());
    //
    //     // Assert
    //     result.Value.Should().BeEquivalentTo(user.ToOutputModel());
    // }
    
    [Fact]
    public async Task RemoveUser_ReturnsNoContent()
    {
        // Arrange
        _userServiceMock
            .Setup(x => x.DeleteUserAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new UserId(It.IsAny<Guid>()));
        
        // Act
        var result = await _userController.DeleteUserAsync(It.IsAny<Guid>());
        
        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}


