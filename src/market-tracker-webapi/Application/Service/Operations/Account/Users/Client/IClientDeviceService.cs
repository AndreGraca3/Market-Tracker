using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Account.Users.Client;

public interface IClientDeviceService
{
    Task<Either<IServiceError, bool>> UpsertNotificationDeviceAsync(Guid clientId, string deviceId, string firebaseToken);

    Task<Either<IServiceError, bool>> DeRegisterNotificationDeviceAsync(Guid id, string deviceId);
}