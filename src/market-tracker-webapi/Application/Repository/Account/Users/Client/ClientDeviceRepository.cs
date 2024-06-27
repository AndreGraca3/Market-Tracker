using market_tracker_webapi.Application.Domain.Schemas.Account;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Account;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Account.Users.Client;

public class ClientDeviceRepository(MarketTrackerDataContext dataContext) : IClientDeviceRepository
{
    public async Task<IEnumerable<DeviceToken>> GetDeviceTokensByClientIdAsync(Guid clientId)
    {
        return await dataContext.FcmRegister
            .Where(r => r.ClientId == clientId)
            .Select(r => r.ToDeviceToken())
            .ToListAsync();
    }

    public Task<DeviceToken?> GetDeviceTokenByDeviceIdAsync(Guid clientId, string deviceId)
    {
        return dataContext.FcmRegister
            .Where(r => r.ClientId == clientId && r.DeviceId == deviceId)
            .Select(r => r.ToDeviceToken())
            .FirstOrDefaultAsync();
    }

    public async Task<DeviceToken> AddDeviceTokenAsync(Guid clientId, string deviceId, string firebaseToken)
    {
        var fcmRegisterEntity = new FcmRegisterEntity
        {
            ClientId = clientId,
            DeviceId = deviceId,
            Token = firebaseToken,
            UpdatedAt = DateTime.UtcNow
        };

        await dataContext.FcmRegister.AddAsync(fcmRegisterEntity);
        await dataContext.SaveChangesAsync();
        return fcmRegisterEntity.ToDeviceToken();
    }

    public async Task<DeviceToken?> UpdateDeviceTokenAsync(Guid clientId, string deviceId, string firebaseToken)
    {
        var fcmRegisterEntity =
            await dataContext.FcmRegister.FirstOrDefaultAsync(r =>
                r.ClientId == clientId && r.DeviceId == deviceId);

        if (fcmRegisterEntity is null)
        {
            return null;
        }

        fcmRegisterEntity.Token = firebaseToken;
        fcmRegisterEntity.UpdatedAt = DateTime.UtcNow;

        await dataContext.SaveChangesAsync();
        return fcmRegisterEntity.ToDeviceToken();
    }

    public async Task<DeviceToken?> RemoveDeviceTokenAsync(Guid clientId, string deviceId)
    {
        var registerEntity =
            await dataContext.FcmRegister.FirstOrDefaultAsync(r => r.ClientId == clientId && r.DeviceId == deviceId);
        if (registerEntity is null)
        {
            return null;
        }

        dataContext.Remove(registerEntity);
        await dataContext.SaveChangesAsync();
        return registerEntity.ToDeviceToken();
    }
}