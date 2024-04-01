using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;

namespace market_tracker_webapi.Application.Repository.Operations.User
{
    public class UserRepository: IUserRepository
    {
        private readonly MarketTrackerDataContext _marketTrackerDataContext;

        public UserRepository(MarketTrackerDataContext marketTrackerDataContext)
        {
            _marketTrackerDataContext = marketTrackerDataContext;
        }

        public async Task<Models.UserData?> GetUserAsync(int id)
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

        private static Models.UserData? MapUserEntity(UserEntity? userEntity)
        {
            return userEntity is not null ? new Models.UserData()
            {
                Id = userEntity.Id,
                Name = userEntity.Name
            } : null;
        }

        Task<Models.UserData?> IUserRepository.GetUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<UserInfoData?> IUserRepository.GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<int> IUserRepository.CreateUserAsync(string name, string userName, string email, string password, string avatarUrl)
        {
            throw new NotImplementedException();
        }

        Task<UserInfoData?> IUserRepository.GetUserByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        Task<Models.UserData?> IUserRepository.GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        Task<UserDetailsData> IUserRepository.UpdateUserAsync(int id, string? name, string? userName, string? avatarUrl)
        {
            throw new NotImplementedException();
        }

        Task IUserRepository.DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<TokenData> IUserRepository.CreateTokenAsync(string tokenValue, int userId)
        {
            throw new NotImplementedException();
        }

        Task<AuthenticatedUserData?> IUserRepository.GetUserAndTokenByTokenValueAsync(string token)
        {
            throw new NotImplementedException();
        }

        Task<TokenData?> IUserRepository.GetTokenByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        Task IUserRepository.UpdateTokenLastUsedAsync(TokenData tokenData, DateTime now)
        {
            throw new NotImplementedException();
        }

        Task IUserRepository.DeleteTokenAsync(string token)
        {
            throw new NotImplementedException();
        }
    }
}
