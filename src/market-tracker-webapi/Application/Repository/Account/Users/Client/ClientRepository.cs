using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Account.Users;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Account.Users.Client;

using Client = Domain.Schemas.Account.Users.Client;

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

    public async Task<Client?> GetClientByIdAsync(Guid id)
    {
        var query = from user in dataContext.User
            join client in dataContext.Client on user.Id equals client.UserId into clientGroup
            from client in clientGroup.DefaultIfEmpty()
            where user.Role == Role.Client.ToString() && user.Id == id
            select new Client(user.Id, client.Username, user.Name, user.Email, user.CreatedAt, client.Avatar);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<Client?> GetClientByUsernameAsync(string username)
    {
        var query = from user in dataContext.User
            join client in dataContext.Client on user.Id equals client.UserId into clientGroup
            from client in clientGroup.DefaultIfEmpty()
            where user.Role == Role.Client.ToString() && client.Username == username
            select new Client(user.Id, client.Username, user.Name, user.Email, user.CreatedAt, client.Avatar);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<UserId> CreateClientAsync(Guid userId, string username, string? avatarUrl)
    {
        var newClient = new ClientEntity
        {
            UserId = userId,
            Username = username,
            Avatar = avatarUrl
        };
        await dataContext.Client.AddAsync(newClient);
        await dataContext.SaveChangesAsync();
        return new UserId(newClient.UserId);
    }

    public async Task<ClientItem?> UpdateClientAsync(Guid id, string? username, string? avatarUrl = null)
    {
        var clientEntity = await dataContext.Client.FindAsync(id);
        if (clientEntity is null)
        {
            return null;
        }

        clientEntity.Username = username ?? clientEntity.Username;
        clientEntity.Avatar = avatarUrl ?? clientEntity.Avatar;

        await dataContext.SaveChangesAsync();
        return clientEntity.ToClientItem();
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
        return await GetClientByIdAsync(id);
    }
}