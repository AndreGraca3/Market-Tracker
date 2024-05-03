using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Client;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Client;

public interface IClientService
{
    Task<Either<IServiceError, PaginatedResult<ClientItem>>> GetClientsAsync(string? username, int skip, int limit);

    Task<Either<UserFetchingError, ClientInfo>> GetClientAsync(Guid id);

    Task<Either<UserCreationError, GuidOutputModel>> CreateClientAsync(
        string username,
        string name,
        string email,
        string? password,
        string? avatarUrl = null
    );

    Task<Either<UserFetchingError, ClientInfo>> UpdateClientAsync(
        Guid id,
        string? name,
        string? username,
        string? avatarUrl
    );

    Task<Either<UserFetchingError, GuidOutputModel>> DeleteClientAsync(Guid id);
}