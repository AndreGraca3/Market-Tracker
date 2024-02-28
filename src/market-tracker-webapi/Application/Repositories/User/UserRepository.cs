using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgresSQLTables;

namespace market_tracker_webapi.Application.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly MarketTrackerDataContext _marketTrackerDataContext;

        public UserRepository(MarketTrackerDataContext marketTrackerDataContext)
        {
            _marketTrackerDataContext = marketTrackerDataContext;
        }

        public async Task<User?> GetUserAsync(int id)
        {
            return MapUserEntity(await _marketTrackerDataContext.User.FindAsync(id));
        }

        public async Task<int> AddUser(string name)
        {
            var newUser = new UserEntity { Name = name };
            await _marketTrackerDataContext.User.AddAsync(newUser);
            await _marketTrackerDataContext.SaveChangesAsync();
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
