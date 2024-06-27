using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Domain.Schemas.List;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.List;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.List;

public class ListRepository(MarketTrackerDataContext context) : IListRepository
{
    public async Task<IEnumerable<ShoppingList>> GetListsFromClientAsync(
        Guid clientId,
        bool? isOwner,
        string? listName = null,
        DateTime? createdAfter = null,
        bool? isArchived = false
    )
    {
        var query = from listEntity in context.List
            join listClient in context.ListClient on listEntity.Id equals listClient.ListId into listClients
            from listClient in listClients.DefaultIfEmpty()
            where (isOwner == true && listEntity.OwnerId == clientId ||
                   isOwner == false && listClient.ClientId == clientId ||
                   isOwner == null && (listEntity.OwnerId == clientId || listClient.ClientId == clientId)
                  ) &&
                  (listName == null || EF.Functions.ILike(listEntity.Name, $"%{listName}%")) &&
                  (createdAfter == null || listEntity.CreatedAt >= createdAfter) &&
                  (isArchived == true && listEntity.ArchivedAt != null ||
                   isArchived == false && listEntity.ArchivedAt == null ||
                   isArchived == null)
            select listEntity;

        return await query
            .OrderByDescending(listEntity => listEntity.CreatedAt)
            .Select(listEntity => new ShoppingList(
                listEntity.Id,
                listEntity.Name,
                listEntity.ArchivedAt,
                listEntity.CreatedAt,
                listEntity.OwnerId,
                context.ListClient.Where(lc =>
                    lc.ListId == listEntity.Id).Select(lc => lc.ClientId).ToList()
            ))
            .ToListAsync();
    }

    public async Task<IEnumerable<ClientItem>> GetClientMembersByListIdAsync(string listId)
    {
        return await (from listClient in context.ListClient
                join client in context.Client on listClient.ClientId equals client.UserId
                where listClient.ListId == listId
                select new ClientItem(client.UserId, client.Username, client.Avatar))
            .ToListAsync();
    }

    public async Task<ShoppingList?> GetListByIdAsync(string id)
    {
        var shoppingList = await (from listEntity in context.List
            join listClient in context.ListClient on listEntity.Id equals listClient.ListId
                into listMembers
            where listEntity.Id == id
            select new
            {
                List = listEntity,
                ClientIds = listMembers
                    .Where(lMembers => lMembers.ListId == listEntity.Id)
                    .Select(lc => lc.ClientId)
                    .ToList()
            }).FirstOrDefaultAsync();

        if (shoppingList == null)
            return null;


        return new ShoppingList(
            shoppingList.List.Id,
            shoppingList.List.Name,
            shoppingList.List.ArchivedAt,
            shoppingList.List.CreatedAt,
            shoppingList.List.OwnerId,
            shoppingList.ClientIds
        );
    }

    public async Task<ShoppingListId> AddListAsync(string listName, Guid ownerId)
    {
        var listEntity = new ListEntity
        {
            Name = listName,
            OwnerId = ownerId,
            CreatedAt = DateTime.UtcNow
        };

        await context.List.AddAsync(listEntity);
        await context.SaveChangesAsync();

        return new ShoppingListId(listEntity.Id);
    }

    public async Task<ShoppingListItem?> UpdateListAsync(string id, DateTime? archivedAt, string? listName = null)
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

        return listEntity.ToShoppingListItem();
    }

    public async Task<ShoppingListItem?> DeleteListAsync(string id)
    {
        var listEntity = await context.List.FindAsync(id);

        if (listEntity == null)
        {
            return null;
        }

        context.List.Remove(listEntity);
        await context.SaveChangesAsync();

        return listEntity.ToShoppingListItem();
    }

    public async Task<ListClient> AddListMemberAsync(string listId, Guid clientId)
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

    public async Task<ListClient?> DeleteListMemberAsync(string listId, Guid clientId)
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