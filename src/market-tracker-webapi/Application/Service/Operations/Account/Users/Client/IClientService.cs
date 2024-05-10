using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Models.Account.Users;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Account.Users.Client;

using Client = Domain.Models.Account.Users.Client;

public interface IClientService
{
    Task<Either<IServiceError, PaginatedResult<ClientItem>>> GetClientsAsync(string? username, int skip, int limit);

    Task<Either<UserFetchingError, Client>> GetClientByIdAsync(Guid id);

    Task<Either<UserCreationError, UserId>> CreateClientAsync(
        string username,
        string name,
        string email,
        string? password,
        string? avatarUrl = null
    );

    Task<Either<UserFetchingError, Client>> UpdateClientAsync(
        Guid id,
        string? name,
        string? username,
        string? avatarUrl
    );

    Task<Either<UserFetchingError, UserId>> DeleteClientAsync(Guid id);
}