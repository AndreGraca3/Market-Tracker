using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.List;

public class ListRepository(MarketTrackerDataContext context) : IListRepository
{
    public async Task<IEnumerable<ListOfProducts>> GetListsAsync(
        Guid clientId,
        string? listName = null,
        DateTime? createdAfter = null,
        bool? isArchived = false,
        bool? isOwner = null
    )
    {
        var query = context.List.AsQueryable()
            .Join(context.ListClient, listEntity => listEntity.Id, listClient => listClient.ListId,
                (listEntity, listClient) => listEntity);

        if (isOwner is not null)
        {
            if (!isOwner.Value)
            {
                query = query.Where(l => l.OwnerId != clientId);
            }
            else
            {
                query = query.Where(l => l.OwnerId == clientId);
            }
        }
        
        if (!string.IsNullOrEmpty(listName))
        {
            query = query.Where(l => EF.Functions.ILike(l.Name, $"%{listName}%"));
        }

        if (isArchived is not null)
        {
            if (!isArchived.Value)
            {
                query = query.Where(l => l.ArchivedAt == null);
            }
            else
            {
                query = query.Where(l => l.ArchivedAt != null);
            }
        }

        if (createdAfter is not null)
        {
            query = query.Where(l => l.CreatedAt >= createdAfter);
        }

        return await query
            .OrderByDescending(listEntity => listEntity.CreatedAt)
            .Select(listEntity => listEntity.ToListOfProducts())
            .ToListAsync();
    }

    public async Task<IEnumerable<Guid>> GetListClientsByListIdAsync(int listId)
    {
        var clients = await context.ListClient
            .Where(listClient => listClient.ListId == listId)
            .Select(listClient => listClient.ClientId)
            .ToListAsync();
        
        return clients;
    }

    public async Task<ListOfProducts?> GetListByIdAsync(int id)
    {
        var listEntity = await context.List.FindAsync(id);
        return listEntity?.ToListOfProducts();
    }

    public async Task<int> AddListAsync(string listName, Guid ownerId)
    {
        var listEntity = new ListEntity()
        {
            Name = listName,
            OwnerId = ownerId
        };

        await context.List.AddAsync(listEntity);
        await context.SaveChangesAsync();

        return listEntity.Id;
    }

    public async Task<ListOfProducts?> UpdateListAsync(int id, DateTime? archivedAt, string? listName = null)
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

        return listEntity.ToListOfProducts();
    }

    public async Task<ListOfProducts?> DeleteListAsync(int id)
    {
        var listEntity = await context.List.FindAsync(id);

        if (listEntity == null)
        {
            return null;
        }

        context.List.Remove(listEntity);
        await context.SaveChangesAsync();

        return listEntity.ToListOfProducts();
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