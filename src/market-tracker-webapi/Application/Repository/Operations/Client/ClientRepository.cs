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
    public async Task<PaginatedResult<ClientInfo>> GetClientsAsync(string? username, int skip, int take)
    {
        var query = from user in dataContext.User
            join client in dataContext.Client on user.Id equals client.UserId into clientGroup
            from client in clientGroup.DefaultIfEmpty()
            where user.Role == "client"
            select new ClientInfo(user.Id, user.Username, user.Name, user.Email, user.CreatedAt, client.Avatar);

        var clients = await query
            .Where(c => username == null || c.Username.Contains(username))
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return new PaginatedResult<ClientInfo>(clients, query.Count(), skip, take);
    }

    public async Task<ClientInfo?> GetClientByIdAsync(Guid id)
    {
        return await (from user in dataContext.User
                join client in dataContext.Client on user.Id equals client.UserId
                where user.Id == id
                select new ClientInfo(user.Id, user.Username, user.Name, user.Email, user.CreatedAt, client.Avatar))
            .FirstOrDefaultAsync();
    }

    public async Task<Guid> CreateClientAsync(Guid userId, string avatarUrl)
    {
        var newClient = new ClientEntity
        {
            UserId = userId,
            Avatar = avatarUrl
        };
        await dataContext.Client.AddAsync(newClient);
        await dataContext.SaveChangesAsync();
        return newClient.UserId;
    }

    public async Task<Client?> UpdateClientAsync(Guid id, string? avatarUrl = null)
    {
        var clientEntity = await dataContext.Client.FindAsync(id);
        if (clientEntity is null)
        {
            return null;
        }

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

    public async Task<bool> UpsertFirebaseTokenAsync(Guid clientId, string deviceId, string firebaseToken)
    {
        var registerEntity =
            await dataContext.FcmRegister.FirstOrDefaultAsync(r => r.ClientId == clientId && r.DeviceId == deviceId);
        if (registerEntity is null)
        {
            registerEntity = new FcmRegisterEntity
            {
                ClientId = clientId,
                DeviceId = deviceId,
                FirebaseToken = firebaseToken
            };
            await dataContext.FcmRegister.AddAsync(registerEntity);
            await dataContext.SaveChangesAsync();
            return true;
        }

        registerEntity.FirebaseToken = firebaseToken;

        await dataContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveFirebaseTokenAsync(Guid id, string deviceId)
    {
        var registerEntity = await dataContext.FcmRegister.FirstOrDefaultAsync(r => r.ClientId == id && r.DeviceId == deviceId);
        if (registerEntity is null)
        {
            return false;
        }

        dataContext.Remove(registerEntity);
        await dataContext.SaveChangesAsync();
        return true;
    }
}