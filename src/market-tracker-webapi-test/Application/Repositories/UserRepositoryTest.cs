using FluentAssertions;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Repositories.User;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi_test.Application.Repositories
{
    public class UserRepositoryTest
    {
        [Fact]
        public async Task GetUserAsync_ReturnsObjectAsync()
        {
            // Mock DB
            var expectedUser = new UserEntity
            {
                Id = new Guid("33333333-3333-3333-3333-333333333333"),
                Name = "André",
                Username = "Graca",
                Email = "André@gmail.com",
                Password = "123"
            };

            var mockEntities = new List<UserEntity>
            {
                new()
                {
                    Id = new Guid("11111111-1111-1111-1111-111111111111"),
                    Name = "Diogo",
                    Username = "Digo",
                    Email = "Diogo@gmail.com",
                    Password = "123"
                },
                new()
                {
                    Id = new Guid("22222222-2222-2222-2222-222222222222"),
                    Name = "Daniel",
                    Username = "Dani",
                    Email = "Daniel@gmail.com",
                    Password = "123"
                },
                expectedUser
            };

            var context = CreateDatabase(mockEntities);
            var userRepo = new UserRepository(context);

            // Act
            var actualUser = await userRepo.GetUserAsync(expectedUser.Id);

            // Assert
            actualUser.Should().BeEquivalentTo(expectedUser);
        }

        [Fact]
        public async Task GetUserAsync_ReturnsNullAsync()
        {
            // Mock DB
            var mockEntities = new List<UserEntity>();

            var context = CreateDatabase(mockEntities);
            var userRepo = new UserRepository(context);

            // Act
            var actualUser = await userRepo.GetUserAsync(new Guid("11111111-1111-1111-1111-111111111111"));

            // Assert
            actualUser.Should().BeNull();
        }

        [Fact]
        public async Task CreateUserAsync_ReturnsObjectAsync()
        {
            // Mock DB
            var mockEntities = new List<UserEntity>();

            var context = CreateDatabase(mockEntities);
            var userRepo = new UserRepository(context);

            // Act
            var actualId = await userRepo.CreateUserAsync("Diogo", "Digo", "Diogo@gmail.com", "123");

            // Assert
            actualId.Should().NotBeEmpty();
        }
        
        [Fact]
        public async Task UpdateUserAsync_ReturnsObjectAsync()
        {
            // Mock DB
            var expectedUserId = new Guid("11111111-1111-1111-1111-111111111111");
            
            var mockEntities = new List<UserEntity>
            {
                new()
                {
                    Id = expectedUserId,
                    Name = "Diogo",
                    Username = "Digo",
                    Email = "Diogo@gmail.com",
                    Password = "123"
                }
            };

            var context = CreateDatabase(mockEntities);
            var userRepo = new UserRepository(context);

            // Act
            var actualUserDetails = await userRepo.UpdateUserAsync(expectedUserId, "Diogo Santos", "DiogoFAS");

            // Assert
            actualUserDetails.Should().BeEquivalentTo(await userRepo.GetUserAsync(expectedUserId));
        }
        
        [Fact]
        public async Task UpdateUserAsync_ReturnsNullAsync()
        {
            // Mock DB
            var mockEntities = new List<UserEntity>();

            var context = CreateDatabase(mockEntities);
            var userRepo = new UserRepository(context);

            // Act
            var actualUserDetails = await userRepo.UpdateUserAsync(new Guid("11111111-1111-1111-1111-111111111111"), "Diogo Santos", "DiogoFAS");

            // Assert
            actualUserDetails.Should().BeNull();
        }
        
        [Fact]
        public async Task DeleteUserAsync_ReturnsObjectAsync()
        {
            // Mock DB
            var expectedDeletedUserData = new DeletedUserData
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                CreatedAt = default
            };

            var mockEntities = new List<UserEntity>
            {
                new()
                {
                    Id = expectedDeletedUserData.Id,
                    Name = "Diogo",
                    Username = "Digo",
                    Email = "Diogo@gmail.com",
                    Password = "123"
                }
            };

            var context = CreateDatabase(mockEntities);
            var userRepo = new UserRepository(context);

            // Act
            var actualDeletedUserData = await userRepo.DeleteUserAsync(expectedDeletedUserData.Id);

            // Assert
            actualDeletedUserData.Should().NotBeNull();
            actualDeletedUserData.Should().BeEquivalentTo(expectedDeletedUserData);
        }

        [Fact]
        public async Task DeleteUserAsync_ReturnsNullAsync()
        {
            // Mock DB
            var mockEntities = new List<UserEntity>();

            var context = CreateDatabase(mockEntities);
            var userRepo = new UserRepository(context);

            // Act
            var actualDeletedUserData = await userRepo.DeleteUserAsync(new Guid("11111111-1111-1111-1111-111111111111"));

            // Assert
            actualDeletedUserData.Should().BeNull();
        }

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