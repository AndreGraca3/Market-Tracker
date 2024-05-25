using market_tracker_webapi.Application.Domain.Schemas.Account;
using market_tracker_webapi.Application.Repository.Account.Users.Client;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Transaction;

namespace market_tracker_webapi.Application.Service.Operations.Account.Users.Client;

public class ClientDeviceService(
    IClientDeviceRepository clientDeviceRepository,
    ITransactionManager transactionManager
) : IClientDeviceService
{
    public async Task<DeviceToken> UpsertNotificationDeviceAsync(Guid clientId, string deviceId, string firebaseToken)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var deviceToken = await clientDeviceRepository.GetDeviceTokenByDeviceIdAsync(clientId, deviceId);

            if (deviceToken is null)
            {
                return await clientDeviceRepository.AddDeviceTokenAsync(clientId, deviceId, firebaseToken);
            }

            return (await clientDeviceRepository.UpdateDeviceTokenAsync(clientId, deviceId, firebaseToken))!;
        });
    }

    public async Task<DeviceToken> DeRegisterNotificationDeviceAsync(Guid id, string deviceId)
    {
        return await transactionManager.ExecuteAsync(async () =>
            await clientDeviceRepository.RemoveDeviceTokenAsync(id, deviceId) ??
            throw new MarketTrackerServiceException(new UserFetchingError.DeviceTokenNotFound(id, deviceId))
        );
    }
}