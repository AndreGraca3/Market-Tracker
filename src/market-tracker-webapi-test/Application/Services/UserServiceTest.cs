using market_tracker_webapi.Application.Http.Controllers;
using market_tracker_webapi.Application.Repository.Operations.User;
using market_tracker_webapi.Application.Service.Operations.User;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace market_tracker_webapi_test.Application.Services;

public class UserServiceTest
{
    //private readonly Mock<IUserRepository> _userRepositoryMock;
    //private readonly UserController _userController;
    //private readonly TransactionManager _transactionManager = new TransactionManager(); 
    //
    //public UserServiceTest()
    //{
    //    _userRepositoryMock = new Mock<IUserRepository>();
    //    _userController = new UserService(_transactionManager, _userRepositoryMock.Object);
    //}
//
    //[Fact]
    //public async Task GetUserAsync_RespondsWith_Ok_ReturnsObjectAsync()
    //{
    //    // Expected Arrange
    //    var expectedUser = new UserData
    //        { Id = new Guid("1"), Name = "Diogo", Email = "Diogo@gmail.com", Password = "123" };
//
    //    // Repository Arrange
    //    _userRepositoryMock
    //        .Setup(repo => repo.GetUserAsync(It.IsAny<Guid>()))
    //        .ReturnsAsync(expectedUser);
//
    //    // Act
    //    var actual = await _userController.GetUserAsync(It.IsAny<Guid>());
//
    //    // Assert
    //    OkObjectResult result = Assert.IsType<OkObjectResult>(actual.Result);
    //    UserData userData = Assert.IsAssignableFrom<UserData>(result.Value);
    //    userData.Should().BeEquivalentTo(expectedUser);
    //}
    //
    //private static MarketTrackerDataContext CreateDatabase(List<UserEntity> userEntities)
    //{
    //    DbContextOptions<MarketTrackerDataContext> options = new DbContextOptionsBuilder<MarketTrackerDataContext>()
    //        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    //        .Options;
//
    //    var databaseContext = new MarketTrackerDataContext(options);
    //    databaseContext.User.AddRange(userEntities);
    //    databaseContext.SaveChanges();
    //    return databaseContext;
    //}
}