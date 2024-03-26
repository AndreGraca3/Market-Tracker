using market_tracker_webapi.Application.Models;

namespace market_tracker_webapi.Application.Repository.Operations.User
{
    public interface IUserRepository
    {
        Task<Models.UserModel?> GetUserAsync(Guid id);

        Task<UserInfoData?> GetUserByIdAsync(int id);

        Task<int> CreateUserAsync(string name, string userName, string email, string password, string avatarUrl);

        Task<UserInfoData?> GetUserByNameAsync(string name);

        Task<Models.UserModel?> GetUserByEmail(string email);

        Task<UserDetailsData> UpdateUserAsync(int id, string? name = null, string? userName = null, string? avatarUrl = null);

        Task DeleteUserAsync(int id);

        Task<TokenData> CreateTokenAsync(string tokenValue, int userId);

        Task<AuthenticatedUserData?> GetUserAndTokenByTokenValueAsync(string token);

        Task<TokenData?> GetTokenByUserIdAsync(int userId);

        Task UpdateTokenLastUsedAsync(TokenData tokenData, DateTime now);

        Task DeleteTokenAsync(string token);
    }
}
