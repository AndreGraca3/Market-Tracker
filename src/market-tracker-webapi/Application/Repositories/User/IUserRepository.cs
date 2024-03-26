using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;

namespace market_tracker_webapi.Application.Repositories.User
{
    public interface IUserRepository
    {
        Task<UserData?> GetUserAsync(Guid id);

        Task<UserInfoData?> GetUserByIdAsync(Guid id);

        Task<Guid> CreateUserAsync(string username, string name, string email, string password);

        Task<UserInfoData?> GetUserByNameAsync(string name);

        Task<UserData?> GetUserByEmail(string email);

        Task<UserDetailsData?> UpdateUserAsync(Guid id, string? name = null, string? userName = null);

        Task<DeletedUserData?> DeleteUserAsync(Guid id);
    }
}