using market_tracker_webapi.Application.Domain.Models.Account.Users;
using market_tracker_webapi.Application.Domain.Models.List;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.List;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.List;

public class ListRepository(MarketTrackerDataContext context) : IListRepository
{
    public async Task<IEnumerable<ShoppingList>> GetListsAsync(
        Guid clientId,
        bool isOwner,
        string? listName = null,
        DateTime? createdAfter = null,
        bool? isArchived = false
    )
    {
        var query = context.List.AsQueryable()
            .Join(context.ListClient, listEntity => listEntity.Id, listClient => listClient.ListId,
                (listEntity, listClient) => listEntity);

        if (isOwner)
        {
            query = query.Where(l => l.OwnerId == clientId);
        }

        if (!string.IsNullOrEmpty(listName))
        {
            query = query.Where(l => EF.Functions.ILike(l.Name, $"%{listName}%"));
        }

        if (isArchived is not null)
        {
            query = !isArchived.Value ? query.Where(l => l.ArchivedAt == null) : query.Where(l => l.ArchivedAt != null);
        }

        if (createdAfter is not null)
        {
            query = query.Where(l => l.CreatedAt >= createdAfter);
        }

        return await query
            .OrderByDescending(listEntity => listEntity.CreatedAt)
            .Select(listEntity => listEntity.ToShoppingList())
            .ToListAsync();
    }

    public async Task<IEnumerable<UserId>> GetClientIdsByListIdAsync(int listId)
    {
        var clients = await context.ListClient
            .Where(listClient => listClient.ListId == listId)
            .Select(listClient => new UserId(listClient.ClientId))
            .ToListAsync();

        return clients;
    }

    public async Task<bool> IsClientInListAsync(int listId, Guid clientId)
    {
        return await context.ListClient
            .AnyAsync(lc => lc.ListId == listId && lc.ClientId == clientId);
    }

    public async Task<ShoppingList?> GetListByIdAsync(int id)
    {
        var listEntity = await context.List.FindAsync(id);
        return listEntity?.ToShoppingList();
    }

    public async Task<ShoppingListId> AddListAsync(string listName, Guid ownerId)
    {
        var listEntity = new ListEntity()
        {
            Name = listName,
            OwnerId = ownerId
        };

        await context.List.AddAsync(listEntity);
        await context.SaveChangesAsync();

        return new ShoppingListId(listEntity.Id);
    }

    public async Task<ShoppingList?> UpdateListAsync(int id, DateTime? archivedAt, string? listName = null)
    {
        var listEntity = await context.List.FindAsync(id);

        if (listEntity == null)
        {
            return null;
        }

        if (listName != null)
        {
            listEntity.Name = listName;
        }

        if (archivedAt != null)
        {
            listEntity.ArchivedAt = archivedAt;
        }

        await context.SaveChangesAsync();

        return listEntity.ToShoppingList();
    }

    public async Task<ShoppingList?> DeleteListAsync(int id)
    {
        var listEntity = await context.List.FindAsync(id);

        if (listEntity == null)
        {
            return null;
        }

        context.List.Remove(listEntity);
        await context.SaveChangesAsync();

        return listEntity.ToShoppingList();
    }

    public async Task<ListClient> AddListClientAsync(int listId, Guid clientId)
    {
        var listClient = new ListClientEntity()
        {
            ListId = listId,
            ClientId = clientId
        };

        await context.ListClient.AddAsync(listClient);
        await context.SaveChangesAsync();

        return listClient.ToListClient();
    }

    public async Task<ListClient?> DeleteListClientAsync(int listId, Guid clientId)
    {
        var listClient = await context.ListClient
            .Where(lc => lc.ListId == listId && lc.ClientId == clientId)
            .FirstOrDefaultAsync();

        if (listClient == null)
        {
            return null;
        }

        context.ListClient.Remove(listClient);
        await context.SaveChangesAsync();

        return listClient.ToListClient();
    }
}