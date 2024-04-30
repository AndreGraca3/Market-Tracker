using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.User;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.User;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.User;

using User = Domain.User;

public interface IUserService
{
    Task<PaginatedResult<UserItem>> GetUsersAsync(string? username, string? role, int skip, int take);

    Task<Either<UserFetchingError, UserOutputModel>> GetUserAsync(Guid id);

    Task<AuthenticatedUser?> GetUserByToken(Guid tokenValue);

    Task<Either<UserCreationError, UserCreationOutputModel>> CreateUserAsync(
        string username,
        string name,
        string email,
        string password,
        string role = "operator"
    );

    Task<Either<UserFetchingError, UserOutputModel>> UpdateUserAsync(
        Guid id,
        string? name,
        string? username
    );

    Task<Either<UserFetchingError, UserOutputModel>> DeleteUserAsync(Guid id);
}