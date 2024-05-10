using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Models.Account.Users;

namespace market_tracker_webapi.Application.Repository.Account.Users.User;

using User = Domain.Models.Account.Users.User;

public interface IUserRepository
{
    Task<PaginatedResult<UserItem>> GetUsersAsync(string? role, int skip, int take);

    Task<User?> GetUserByIdAsync(Guid id);
    
    Task<User?> GetUserByEmailAsync(string email);

    Task<Guid> CreateUserAsync(string name, string email, string role);

    Task<User?> UpdateUserAsync(Guid id, string? name = null);

    Task<User?> DeleteUserAsync(Guid id);
}