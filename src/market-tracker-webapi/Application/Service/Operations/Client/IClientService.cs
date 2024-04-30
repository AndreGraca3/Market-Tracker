using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Client;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Client;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Client;

using Client = Domain.Client;

public interface IClientService
{
    Task<PaginatedResult<ClientInfo>> GetClientsAsync(string? username, int skip, int limit);

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
        string avatarUrl
    );
}