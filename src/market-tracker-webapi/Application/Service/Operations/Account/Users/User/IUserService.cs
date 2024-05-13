using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;

namespace market_tracker_webapi.Application.Service.Operations.Account.Users.User;

using User = Domain.Schemas.Account.Users.User;

public interface IUserService
{
    Task<PaginatedResult<UserItem>> GetUsersAsync(string? role, int skip, int take);

    Task<User> GetUserAsync(Guid id);

    Task<AuthenticatedUser?> GetUserByToken(Guid tokenValue);

    Task<UserId> CreateUserAsync(
        string name,
        string email,
        string password,
        string role
    );

    Task<User> UpdateUserAsync(
        Guid id,
        string? name
    );

    Task<UserId> DeleteUserAsync(Guid id);
}