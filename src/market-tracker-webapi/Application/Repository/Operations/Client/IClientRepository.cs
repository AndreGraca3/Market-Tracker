using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Client;

namespace market_tracker_webapi.Application.Repository.Operations.Client;

using Client = Domain.Client;

public interface IClientRepository
{
    Task<PaginatedResult<ClientInfo>> GetClientsAsync(string? username, int skip, int take);
    
    Task<ClientInfo?> GetClientByIdAsync(Guid id);
    
    Task<Guid> CreateClientAsync(Guid userId, string avatarUrl);

    Task<Client?> UpdateClientAsync(Guid id, string? avatarUrl = null);

    Task<Client?> DeleteClientAsync(Guid id);

    Task<IEnumerable<DeviceToken>> GetDeviceTokensAsync(Guid clientId);

    Task<DeviceToken> UpsertDeviceTokenAsync(Guid clientId, string deviceId, string firebaseToken);
    
    Task<DeviceToken?> RemoveDeviceTokenAsync(Guid clientId, string deviceId);
}