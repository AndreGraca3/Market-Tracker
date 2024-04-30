using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.User;

namespace market_tracker_webapi.Application.Repository.Operations.User;

using User = Domain.User;

public interface IUserRepository
{
    Task<PaginatedResult<UserItem>> GetUsersAsync(string? username, string? role, int skip, int take);

    Task<User?> GetUserByIdAsync(Guid id);

    Task<User?> GetUserByUsernameAsync(string username);

    Task<User?> GetUserByEmailAsync(string email);

    Task<Guid> CreateUserAsync(string username, string name, string email, string role);

    Task<User?> UpdateUserAsync(Guid id, string? name = null, string? userName = null);

    Task<User?> DeleteUserAsync(Guid id);
}