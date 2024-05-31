using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;

namespace market_tracker_webapi.Application.Service.Operations.Account.Users.Client;

using Client = Domain.Schemas.Account.Users.Client;

public interface IClientService
{
    Task<PaginatedResult<ClientItem>> GetClientsAsync(string? username, int skip, int limit);

    Task<Client> GetClientByIdAsync(Guid id);

    Task<UserId> CreateClientAsync(
        string username,
        string name,
        string email,
        string? password,
        string? avatarUrl = null
    );

    Task<Client> UpdateClientAsync(
        Guid id,
        string? name,
        string? username,
        string? avatarUrl
    );

    Task<UserId> DeleteClientAsync(Guid id);
}