using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Models.Account.Users;
using market_tracker_webapi.Application.Http.Controllers.Account;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.User;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Account.Users.User;

using User = Domain.Models.Account.Users.User;

public interface IUserService
{
    Task<Either<IServiceError, PaginatedResult<UserItem>>> GetUsersAsync(string? role, int skip,
        int take);

    Task<Either<UserFetchingError, User>> GetUserAsync(Guid id);

    Task<AuthenticatedUser?> GetUserByToken(Guid tokenValue);

    Task<Either<UserCreationError, UserId>> CreateUserAsync(
        string name,
        string email,
        string password,
        string role
    );

    Task<Either<UserFetchingError, User>> UpdateUserAsync(
        Guid id,
        string? name
    );

    Task<Either<UserFetchingError, UserId>> DeleteUserAsync(Guid id);
}