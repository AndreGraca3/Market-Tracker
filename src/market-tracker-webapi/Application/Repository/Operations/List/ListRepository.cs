using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.List;

public class ListRepository(MarketTrackerDataContext context) : IListRepository
{
    private const double SimilarityThreshold = 0.3;

    public async Task<IEnumerable<ListOfProducts>> GetListsAsync(
        Guid clientId, 
        string? listName = null, 
        DateTime? archivedAt = null, 
        DateTime? createdAt = null)
    {
        var query = context.List.AsQueryable()
            .Where(l => l.ClientId == clientId);
    
        if (!string.IsNullOrEmpty(listName))
        {
            query = query.Where(l => EF.Functions.TrigramsSimilarity(l.Name, listName) > SimilarityThreshold);
        }
    
        if (archivedAt != null)
        {
            query = query.Where(l => l.ArchivedAt == archivedAt);
        }
    
        if (createdAt != null)
        {
            query = query.Where(l => l.CreatedAt == createdAt);
        }
    
        return await query
            .OrderByDescending(listEntity => listEntity.CreatedAt)
            .Select(listEntity => listEntity.ToListOfProducts())
            .ToListAsync();
    }

    public async Task<ListOfProducts?> GetListByIdAsync(int id)
    {
        var listEntity = await context.List.FindAsync(id);
        return listEntity?.ToListOfProducts();
    }

    public async Task<int> AddListAsync(Guid clientId, string listName)
    {
        var listEntity = new ListEntity()
        {
            ClientId = clientId,
            Name = listName
        };
        
        await context.List.AddAsync(listEntity);
        await context.SaveChangesAsync();
        
        return listEntity.Id;
    }

    public async Task<ListOfProducts?> UpdateListAsync(int id, string? listName = null, DateTime? archivedAt = null)
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
        
        var productInListEntities = await context.ListEntry
            .Where(pil => pil.ListId == id)
            .ToListAsync();
        
        context.ListEntry.RemoveRange(productInListEntities);
        context.List.Remove(listEntity);
        await context.SaveChangesAsync();
        
        return listEntity.ToListOfProducts();
    }
}