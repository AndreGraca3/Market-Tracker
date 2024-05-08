using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Models.Account;
using market_tracker_webapi.Application.Http.Models.Client;

namespace market_tracker_webapi.Application.Repository.Operations.Account.Users.Client;

using Client = Domain.Models.Account.Users.Client.Client;

public interface IClientRepository
{
    Task<PaginatedResult<ClientOutputModel>> GetClientsAsync(string? username, int skip, int limit);

    Task<ClientInfo?> GetClientByIdAsync(Guid id);

    Task<Client?> GetClientByUsernameAsync(string username);

    Task<Guid> CreateClientAsync(Guid userId, string username, string? avatarUrl);

    Task<Client?> UpdateClientAsync(Guid id, string? username, string? avatarUrl = null);
    
    Task<Client?> DeleteClientAsync(Guid id);

    Task<IEnumerable<DeviceToken>> GetDeviceTokensByClientIdAsync(Guid clientId);

    Task<DeviceToken> UpsertDeviceTokenAsync(Guid clientId, string deviceId, string firebaseToken);
    
    Task<DeviceToken?> RemoveDeviceTokenAsync(Guid clientId, string deviceId);
}