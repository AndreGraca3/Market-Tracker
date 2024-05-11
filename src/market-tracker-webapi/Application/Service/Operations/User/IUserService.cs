using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.User;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.User;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.User;

using User = Domain.User;

public interface IUserService
{
    Task<Either<IServiceError, PaginatedResult<UserItem>>> GetUsersAsync(string? role, int skip,
        int take);

    Task<Either<UserFetchingError, UserOutputModel>> GetUserAsync(Guid id);

    Task<AuthenticatedUser?> GetUserByToken(Guid tokenValue);

    Task<Either<UserCreationError, UserCreationOutputModel>> CreateUserAsync(
        string name,
        string email,
        string password,
        string role
    );

    Task<Either<UserFetchingError, UserOutputModel>> UpdateUserAsync(
        Guid id,
        string? name
    );

    Task<Either<UserFetchingError, GuidOutputModel>> DeleteUserAsync(Guid id);
}