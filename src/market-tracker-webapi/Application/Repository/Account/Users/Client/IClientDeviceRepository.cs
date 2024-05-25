using market_tracker_webapi.Application.Domain.Schemas.Account;

namespace market_tracker_webapi.Application.Repository.Account.Users.Client;

public interface IClientDeviceRepository
{
    Task<IEnumerable<DeviceToken>> GetDeviceTokensByClientIdAsync(Guid clientId);

    Task<DeviceToken?> GetDeviceTokenByDeviceIdAsync(Guid clientId, string deviceId);

    Task<DeviceToken> AddDeviceTokenAsync(Guid clientId, string deviceId, string firebaseToken);
    
    Task<DeviceToken?> UpdateDeviceTokenAsync(Guid clientId, string deviceId, string firebaseToken);
    
    Task<DeviceToken?> RemoveDeviceTokenAsync(Guid clientId, string deviceId);
}