using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgresSQLTables;

namespace market_tracker_webapi.Application.Repository
{
    public class UserRepository(MarketTrackerDataContext marketTrackerDataContext) : IUserRepository
    {   
        public async Task<User?> GetUser(int id)
        {
            return MapUserEntity(await marketTrackerDataContext.User.FindAsync(id));
        }

        public async Task<int> AddUser(string name)
        {
            var newUser = new UserEntity { Name = name };
            await marketTrackerDataContext.User.AddAsync(newUser);
            await marketTrackerDataContext.SaveChangesAsync();
            return newUser.Id;
        }

        private static User? MapUserEntity(UserEntity? userEntity)
        {
            return userEntity is not null ? new User()
            {
                Id = userEntity.Id,
                Name = userEntity.Name
            } : null;
        }
    }
}
