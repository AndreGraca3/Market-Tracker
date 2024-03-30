using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;

namespace market_tracker_webapi.Application.Repository.Operations.User
{
    public class UserRepository(MarketTrackerDataContext marketTrackerDataContext) : IUserRepository
    {
        public async Task<UserModel?> GetUserAsync(Guid id)
        {
            return MapUserEntity(await marketTrackerDataContext.User.FindAsync(id));
        }

        public async Task<Guid> AddUser(string name)
        {
            var newUser = new UserEntity { Name = name };
            await marketTrackerDataContext.User.AddAsync(newUser);
            await marketTrackerDataContext.SaveChangesAsync();
            return newUser.Id;
        }

        private static UserModel? MapUserEntity(UserEntity? userEntity)
        {
            return userEntity is not null
                ? new UserModel() { Id = userEntity.Id, Name = userEntity.Name }
                : null;
        }

        Task<UserInfoData?> IUserRepository.GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<int> IUserRepository.CreateUserAsync(
            string name,
            string userName,
            string email,
            string password,
            string avatarUrl
        )
        {
            throw new NotImplementedException();
        }

        Task<UserInfoData?> IUserRepository.GetUserByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        Task<UserModel?> IUserRepository.GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        Task<UserDetailsData> IUserRepository.UpdateUserAsync(
            int id,
            string? name,
            string? userName,
            string? avatarUrl
        )
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

        Task<AuthenticatedUser?> IUserRepository.GetUserAndTokenByTokenValueAsync(string token)
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
