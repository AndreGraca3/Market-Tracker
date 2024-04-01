using FluentAssertions;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi_test.Application.Repositories
{
    public class UserRepositoryTest
    {
        // [Fact]
        // public async Task GetUserAsync_ReturnsObjectAsync()
        // {
        //     // Mock DB
        //     var mockEntities = new List<UserEntity>
        //     {
        //         new ()
        //         {
        //             Name = "Diogo"
        //         },
        //         new ()
        //         {
        //             Name = "Daniel"
        //         },
        //         new ()
        //         {
        //             Name = "André"
        //         }
        //     };
        //
        //     var expectedUser = new UserEntity()
        //     {
        //         Name = "André"
        //     };
        //
        //
        //     var context = CreateDatabase(mockEntities);
        //     var userRepo = new UserRepository(context);
        //
        //     // Act
        //     var actualUser = await userRepo.GetUserAsync(expectedUser.Id);
        //
        //     // Assert
        //     actualUser.Should().BeEquivalentTo(expectedUser);
        // }

        // public async Task GetUserAsync_ReturnsNullAsync()
        // {
        //
        // }

        private static MarketTrackerDataContext CreateDatabase(List<UserEntity> userEntities)
        {
            DbContextOptions<MarketTrackerDataContext> options = new DbContextOptionsBuilder<MarketTrackerDataContext>()
                        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                        .Options;

            var databaseContext = new MarketTrackerDataContext(options);
            databaseContext.User.AddRange(userEntities);
            databaseContext.SaveChanges();
            return databaseContext;
        }
    }
}
