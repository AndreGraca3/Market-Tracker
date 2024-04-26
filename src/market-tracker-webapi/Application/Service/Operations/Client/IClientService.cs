using market_tracker_webapi.Application.Http.Models.Client;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Client;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Client;

using Client = Domain.Client;

public interface IClientService
{
    Task<PaginatedResult<ClientInfo>> GetClientsAsync(string? username, int skip, int take);

    Task<Either<UserFetchingError, ClientOutputModel>> GetClientAsync(Guid id);

    Task<Either<UserCreationError, ClientCreationOutputModel>> CreateClientAsync(
        string username,
        string name,
        string email,
        string? password,
        string? avatarUrl = null
    );

    Task<Either<UserFetchingError, Client>> UpdateClientAsync(
        Guid id,
        string avatarUrl
    );

    Task<Either<UserFetchingError, ClientOutputModel>> DeleteClientAsync(Guid id);
}