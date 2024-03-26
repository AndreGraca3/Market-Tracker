using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;

namespace market_tracker_webapi.Application.Repositories.User
{
    public class UserRepository(MarketTrackerDataContext marketTrackerDataContext) : IUserRepository
    {
        public async Task<UserData?> GetUserAsync(Guid id)
        {
            return MapUserEntity(await marketTrackerDataContext.User.FindAsync(id));
        }

        public async Task<UserInfoData?> GetUserByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> CreateUserAsync(string name, string username, string email, string password)
        {
            var newUser = new UserEntity
            {
                Name = name,
                Username = username,
                Email = email,
                Password = password
            };
            await marketTrackerDataContext.User.AddAsync(newUser);
            await marketTrackerDataContext.SaveChangesAsync();
            return newUser.Id;
        }

        Task<UserInfoData?> IUserRepository.GetUserByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        Task<UserData?> IUserRepository.GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDetailsData?> UpdateUserAsync(Guid id, string? name, string? userName)
        {
            var user = await marketTrackerDataContext.User.FindAsync(id);
            if (user is null)
            {
                return null;
            }

            user.Name = name ?? user.Name  ;
            user.Username = userName ?? user.Username;
            
            await marketTrackerDataContext.SaveChangesAsync();
            return new UserDetailsData
            {
                Name = user.Name,
                UserName = user.Username
            };
        }

        public async Task<DeletedUserData?> DeleteUserAsync(Guid id)
        {
            var deletedUser = await marketTrackerDataContext.User.FindAsync(id);
            if (deletedUser is null)
            {
                return null;
            }

            marketTrackerDataContext.Remove(deletedUser);
            await marketTrackerDataContext.SaveChangesAsync();
            return new DeletedUserData
            {
                Id = id, 
                CreatedAt = deletedUser.CreatedAt
            };
        }

        private static UserData? MapUserEntity(UserEntity? userEntity)
        {
            return userEntity is not null
                ? new UserData
                {
                    Id = userEntity.Id,
                    Name = userEntity.Name,
                    Email = userEntity.Email,
                    Password = userEntity.Password,
                    Username = userEntity.Username,
                    CreatedAt = userEntity.CreatedAt
                }
                : null;
        }
    }
}