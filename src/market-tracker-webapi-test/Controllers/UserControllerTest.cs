using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Controllers;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.User;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Operations.User;
using market_tracker_webapi.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using UserOutputModel = market_tracker_webapi.Application.Http.Models.User.UserOutputModel;

namespace market_tracker_webapi_test.Controllers
{
    /*public class UserControllerTest
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _userController;
        private readonly NullLogger<UserController> _loggerMock = new();

        public UserControllerTest()
        {
            _userServiceMock = new Mock<IUserService>();
            _userController = new UserController(_userServiceMock.Object, _loggerMock);
        }

        [Fact]
        public async Task GetUsersAsync_RespondsWith_Ok_ReturnsObjectAsync()
        {
            // Expected Arrange
            var expectedUsers = new List<UserOutputModel>
            {
                new(
                    new Guid("11111111-1111-1111-1111-111111111111"),
                    "DigoFAS",
                    "Diogo",
                    DateTime.Now
                ),
                new(
                    new Guid("22222222-2222-2222-2222-222222222222"),
                    "Graca",
                    "andré",
                    DateTime.Now
                ),
                new(
                    new Guid("33333333-3333-3333-3333-333333333333"),
                    "PSDigo",
                    "Diogo",
                    DateTime.Now
                )
            };

            // Service Arrange
            _userServiceMock
                .Setup(service =>
                    service.GetUsersAsync(It.IsAny<string>(), new Pagination(It.IsAny<int>, It.IsAny<int>())))
                .ReturnsAsync(
                    expectedUsers
                );

            // Act
            var actual = await _userController.GetUsersAsync(It.IsAny<string>());

            // Assert
            OkObjectResult result = Assert.IsType<OkObjectResult>(actual.Result);
            List<UserOutputModel> usersOutputModel = Assert.IsAssignableFrom<List<UserOutputModel>>(result.Value);
            usersOutputModel.Should().BeEquivalentTo(expectedUsers);
        }

        [Fact]
        public async Task GetUserAsync_RespondsWith_Ok_ReturnsObjectAsync()
        {
            // Expected Arrange
            var expectedUser = new UserOutputModel(
                new Guid("11111111-1111-1111-1111-111111111111"),
                "Diogo",
                "Digo",
                DateTime.Now
            );

            // Service Arrange
            _userServiceMock
                .Setup(service => service.GetUserAsync(It.IsAny<Guid>()))
                .ReturnsAsync(
                    EitherExtensions.Success<UserFetchingError, UserOutputModel>(expectedUser)
                );

            // Act
            var actual = await _userController.GetUserAsync(It.IsAny<Guid>());

            // Assert
            OkObjectResult result = Assert.IsType<OkObjectResult>(actual.Result);
            UserOutputModel userOutputModel = Assert.IsAssignableFrom<UserOutputModel>(result.Value);
            userOutputModel.Should().BeEquivalentTo(expectedUser);
        }

        [Fact]
        public async Task GetUserAsync_RespondsWith_NotFound_ReturnsUserProblem()
        {
            // Service Arrange
            _userServiceMock
                .Setup(service => service.GetUserAsync(It.IsAny<Guid>()))
                .ReturnsAsync(
                    EitherExtensions.Failure<UserFetchingError, UserOutputModel>(
                        new UserFetchingError.UserByIdNotFound(It.IsAny<Guid>()))
                );

            // Act
            var actual = await _userController.GetUserAsync(It.IsAny<Guid>());

            // Assert
            ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
            UserProblem.UserByIdNotFound problem =
                Assert.IsAssignableFrom<UserProblem.UserByIdNotFound>(result.Value);
            problem.Status.Should().Be(404);
        }

        [Fact]
        public async Task CreateUserAsync_RespondsWith_Created_ReturnsObjectAsync()
        {
            // Expected Arrange
            var expectedId = new IdOutputModel(new Guid("11111111-1111-1111-1111-111111111111"));

            // Service Arrange
            _userServiceMock
                .Setup(service => service.CreateUserAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()
                ))
                .ReturnsAsync(
                    EitherExtensions.Success<UserCreationError, IdOutputModel>(
                        expectedId
                    )
                );

            // Act
            var actual = await _userController.CreateUserAsync(new UserCreationInputModel(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            ));

            // Assert
            CreatedResult result = Assert.IsType<CreatedResult>(actual.Result);
            IdOutputModel idOutputModel = Assert.IsAssignableFrom<IdOutputModel>(result.Value);
            idOutputModel.Should().BeEquivalentTo(expectedId);
        }

        [Fact]
        public async Task CreateUserAsync_RespondsWith_BadRequest_ReturnsUserProblem()
        {
            // Service Arrange
            _userServiceMock
                .Setup(service => service.CreateUserAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()
                ))
                .ReturnsAsync(
                    EitherExtensions.Failure<UserCreationError, IdOutputModel>(
                        new UserCreationError.InvalidEmail(It.IsAny<string>())
                    )
                );

            // Act
            var actual = await _userController.CreateUserAsync(new UserCreationInputModel(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            ));

            // Assert
            ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
            UserProblem.InvalidEmail problem =
                Assert.IsAssignableFrom<UserProblem.InvalidEmail>(result.Value);
            problem.Status.Should().Be(400);
        }

        [Fact]
        public async Task CreateUserAsync_RespondsWith_Conflict_ReturnsUserProblem()
        {
            // Service Arrange
            _userServiceMock
                .Setup(service => service.CreateUserAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()
                ))
                .ReturnsAsync(
                    EitherExtensions.Failure<UserCreationError, IdOutputModel>(
                        new UserCreationError.EmailAlreadyInUse(It.IsAny<string>())
                    )
                );

            // Act
            var actual = await _userController.CreateUserAsync(new UserCreationInputModel(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            ));

            // Assert
            ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
            UserProblem.UserAlreadyExists problem =
                Assert.IsAssignableFrom<UserProblem.UserAlreadyExists>(result.Value);
            problem.Status.Should().Be(409);
        }

        [Fact]
        public async Task UpdateUserAsync_RespondsWith_Ok_ReturnsObjectAsync()
        {
            // Expected Arrange
            var expectedUser = new UserOutputModel(
                new Guid("11111111-1111-1111-1111-111111111111"),
                "Digo",
                "Diogo"
            );

            // Service Arrange
            _userServiceMock
                .Setup(service => service.UpdateUserAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()
                ))
                .ReturnsAsync(
                    EitherExtensions.Success<UserFetchingError, UserOutputModel>(
                        expectedUser
                    )
                );

            // Act
            var actual = await _userController.UpdateUserAsync(expectedUser.Id, new UserUpdateInputModel(
                It.IsAny<string>(),
                It.IsAny<string>()
            ));

            // Assert
            OkObjectResult result = Assert.IsType<OkObjectResult>(actual.Result);
            UserOutputModel userOutputModel = Assert.IsAssignableFrom<UserOutputModel>(result.Value);
            userOutputModel.Should().BeEquivalentTo(expectedUser);
        }

        [Fact]
        public async Task UpdateUserAsync_RespondsWith_NotFound_ReturnsUserProblem()
        {
            // Service Arrange
            _userServiceMock
                .Setup(service => service.UpdateUserAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()
                ))
                .ReturnsAsync(
                    EitherExtensions.Failure<UserFetchingError, UserOutputModel>(
                        new UserFetchingError.UserByIdNotFound(It.IsAny<Guid>())
                    )
                );

            // Act
            var actual = await _userController.UpdateUserAsync(
                It.IsAny<Guid>(),
                new UserUpdateInputModel(
                    It.IsAny<string>(),
                    It.IsAny<string>()
                )
            );

            // Assert
            ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
            UserProblem.UserByIdNotFound problem =
                Assert.IsAssignableFrom<UserProblem.UserByIdNotFound>(result.Value);
            problem.Status.Should().Be(404);
        }

        [Fact]
        public async Task DeleteUserAsync_RespondsWith_Ok_ReturnsObjectAsync()
        {
            // Expected Arrange
            var expectedUser = new UserOutputModel(
                new Guid("11111111-1111-1111-1111-111111111111"),
                "Digo",
                "Diogo"
            );

            // Service Arrange
            _userServiceMock
                .Setup(service => service.DeleteUserAsync(
                    It.IsAny<Guid>()
                ))
                .ReturnsAsync(
                    EitherExtensions.Success<UserFetchingError, UserOutputModel>(
                        expectedUser
                    )
                );

            // Act
            var actual = await _userController.DeleteUserAsync(It.IsAny<Guid>());

            // Assert
            OkObjectResult result = Assert.IsType<OkObjectResult>(actual.Result);
            UserOutputModel userOutputModel = Assert.IsAssignableFrom<UserOutputModel>(result.Value);
            userOutputModel.Should().BeEquivalentTo(expectedUser);
        }

        [Fact]
        public async Task DeleteUserAsync_RespondsWith_NotFound_ReturnsUserProblem()
        {
            // Service Arrange
            _userServiceMock
                .Setup(service => service.DeleteUserAsync(
                    It.IsAny<Guid>()
                ))
                .ReturnsAsync(
                    EitherExtensions.Failure<UserFetchingError, UserOutputModel>(
                        new UserFetchingError.UserByIdNotFound(It.IsAny<Guid>())
                    )
                );

            // Act
            var actual = await _userController.DeleteUserAsync(
                It.IsAny<Guid>()
            );

            // Assert
            ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
            UserProblem.UserByIdNotFound problem =
                Assert.IsAssignableFrom<UserProblem.UserByIdNotFound>(result.Value);
            problem.Status.Should().Be(404);
        }
    }*/
}
