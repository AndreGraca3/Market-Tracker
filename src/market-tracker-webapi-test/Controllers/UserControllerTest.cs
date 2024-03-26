using FluentAssertions;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Repositories.User;
using market_tracker_webapi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace market_tracker_webapi_test.Controllers
{
    public class UserControllerTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserController _userController;
        private readonly NullLogger<UserController> _loggerMock = new();

        public UserControllerTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userController = new UserController(_userRepositoryMock.Object, _loggerMock);
        }

        /*[Fact]
        public async Task GetUserAsync_RespondsWith_Ok_ReturnsObjectAsync()
        {
            // Expected Arrange
            var expectedUser = new UserData
                { Id = new Guid("1"), Name = "Diogo", Email = "Diogo@gmail.com", Password = "123" };

            // Repository Arrange 
            _userRepositoryMock
                .Setup(repo => repo.GetUserAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedUser);

            // Act
            var actual = await _userController.GetUserAsync(It.IsAny<Guid>());

            // Assert
            OkObjectResult result = Assert.IsType<OkObjectResult>(actual.Result);
            UserData userData = Assert.IsAssignableFrom<UserData>(result.Value);
            userData.Should().BeEquivalentTo(expectedUser);
        }*/
    }
}