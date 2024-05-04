using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Client;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.Client;

using Client = Domain.Client;

public class ClientRepository(
    MarketTrackerDataContext dataContext
) : IClientRepository
{
    public async Task<PaginatedResult<ClientItem>> GetClientsAsync(string? username, int skip, int take)
    {
        var query = from user in dataContext.User
            join client in dataContext.Client on user.Id equals client.UserId into clientGroup
            from client in clientGroup.DefaultIfEmpty()
            where user.Role == Role.Client.ToString() && (username == null || client.Username.Contains(username))
            select new ClientItem(user.Id, client.Username, client.Avatar);

        var clients = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return new PaginatedResult<ClientItem>(clients, query.Count(), skip, take);
    }

    public async Task<ClientInfo?> GetClientByIdAsync(Guid id)
    {
        var query = from user in dataContext.User
            join client in dataContext.Client on user.Id equals client.UserId into clientGroup
            from client in clientGroup.DefaultIfEmpty()
            where user.Role == Role.Client.ToString() && user.Id == id
            select new ClientInfo(user.Id, client.Username, user.Name, user.Email, user.CreatedAt, client.Avatar);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<Client?> GetClientByUsernameAsync(string username)
    {
        return (await dataContext.Client.FirstOrDefaultAsync(client => client.Username == username))?.ToClient();
    }

    public async Task<Guid> CreateClientAsync(Guid userId, string username, string? avatarUrl)
    {
        var newClient = new ClientEntity
        {
            UserId = userId,
            Username = username,
            Avatar = avatarUrl
        };
        await dataContext.Client.AddAsync(newClient);
        await dataContext.SaveChangesAsync();
        return newClient.UserId;
    }

    public async Task<Client?> UpdateClientAsync(Guid id, string? username, string? avatarUrl = null)
    {
        var clientEntity = await dataContext.Client.FindAsync(id);
        if (clientEntity is null)
        {
            return null;
        }

        clientEntity.Username = username ?? clientEntity.Username;
        clientEntity.Avatar = avatarUrl ?? clientEntity.Avatar;

        await dataContext.SaveChangesAsync();
        return clientEntity.ToClient();
    }

    public async Task<Client?> DeleteClientAsync(Guid id)
    {
        var deletedClientEntity = await dataContext.Client.FindAsync(id);
        if (deletedClientEntity is null)
        {
            return null;
        }

        dataContext.Remove(deletedClientEntity);
        await dataContext.SaveChangesAsync();
        return deletedClientEntity.ToClient();
    }

    public async Task<IEnumerable<DeviceToken>> GetDeviceTokensByClientIdAsync(Guid clientId)
    {
        return await dataContext.FcmRegister
            .Where(r => r.ClientId == clientId)
            .Select(r => r.ToDeviceToken())
            .ToListAsync();
    }

    public async Task<DeviceToken> UpsertDeviceTokenAsync(Guid clientId, string deviceId, string firebaseToken)
    {
        var fcmRegisterEntity =
            await dataContext.FcmRegister.FirstOrDefaultAsync(r => r.ClientId == clientId && r.DeviceId == deviceId);
        if (fcmRegisterEntity is null)
        {
            fcmRegisterEntity = new FcmRegisterEntity
            {
                ClientId = clientId,
                DeviceId = deviceId,
                Token = firebaseToken
            };
            await dataContext.FcmRegister.AddAsync(fcmRegisterEntity);
            await dataContext.SaveChangesAsync();
            return fcmRegisterEntity.ToDeviceToken();
        }

        fcmRegisterEntity.Token = firebaseToken;
        fcmRegisterEntity.UpdatedAt = DateTime.Now;

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