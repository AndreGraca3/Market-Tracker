using market_tracker_webapi.Application.Repository.Account.Users.Client;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Account.Users.Client;

public class ClientDeviceService(
    IClientDeviceRepository clientDeviceRepository,
    ITransactionManager transactionManager
) : IClientDeviceService
{
    public async Task<Either<IServiceError, bool>> UpsertNotificationDeviceAsync(Guid clientId, string deviceId,
        string firebaseToken)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var deviceToken = await clientDeviceRepository.GetDeviceTokenByDeviceIdAsync(clientId, deviceId);

            if (deviceToken is null)
            {
                await clientDeviceRepository.AddDeviceTokenAsync(clientId, deviceId, firebaseToken);
            }
            else
            {
                await clientDeviceRepository.UpdateDeviceTokenAsync(clientId, deviceId, firebaseToken);
            }

            return EitherExtensions.Success<IServiceError, bool>(true);
        });
    }

    public async Task<Either<IServiceError, bool>> DeRegisterNotificationDeviceAsync(Guid id, string deviceId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            await clientDeviceRepository.RemoveDeviceTokenAsync(id, deviceId);
            return EitherExtensions.Success<IServiceError, bool>(true);
        });
    }
}