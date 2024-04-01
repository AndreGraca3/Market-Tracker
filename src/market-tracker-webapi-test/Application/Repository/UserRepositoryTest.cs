namespace market_tracker_webapi_test.Application.Repository
{
    public class UserRepositoryTest
    {/*
        [Fact]
        public async Task GetUserAsync_ReturnsObjectAsync()
        {
            // Mock DB
            var mockEntities = new List<UserEntity>
            {
                new ()
                {
                    Id = 1,
                    Name = "Diogo"
                },
                new ()
                {
                    Id = 2,
                    Name = "Daniel"
                },
                new ()
                {
                    Id = 3,
                    Name = "André"
                }
            };

            var expectedUser = new UserEntity()
            {
                Id = 3,
                Name = "André"
            };


            var context = CreateDatabase(mockEntities);
            var userRepo = new UserRepository(context);

            // Act
            var actualUser = await userRepo.GetUserAsync(expectedUser.Id);

            // Assert
            actualUser.Should().BeEquivalentTo(expectedUser);
        }

        public async Task GetUserAsync_ReturnsNullAsync()
        {

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
        }*/
    }
}
