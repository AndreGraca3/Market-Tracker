using market_tracker_webapi.Application.Domain.Schemas.Account;

namespace market_tracker_webapi.Application.Service.Operations.Account.Users.Client;

public interface IClientDeviceService
{
    Task<DeviceToken> UpsertNotificationDeviceAsync(Guid clientId, string deviceId, string firebaseToken);

    Task<DeviceToken> DeRegisterNotificationDeviceAsync(Guid id, string deviceId);
}