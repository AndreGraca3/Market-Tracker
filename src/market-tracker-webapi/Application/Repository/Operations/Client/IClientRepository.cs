using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Client;

namespace market_tracker_webapi.Application.Repository.Operations.Client;

using Client = Domain.Client;

public interface IClientRepository
{
    Task<PaginatedResult<ClientItem>> GetClientsAsync(string? username, int skip, int limit);

    Task<ClientInfo?> GetClientByIdAsync(Guid id);

    Task<Client?> GetClientByUsernameAsync(string username);

    Task<Guid> CreateClientAsync(Guid userId, string username, string? avatarUrl);

    Task<Client?> UpdateClientAsync(Guid id, string? username, string? avatarUrl = null);
}