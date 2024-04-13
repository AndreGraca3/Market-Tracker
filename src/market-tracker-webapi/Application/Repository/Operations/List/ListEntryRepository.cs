using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.List;

public class ListEntryRepository(MarketTrackerDataContext context) : IListEntryRepository
{
    private const double SimilarityThreshold = 0.3;
    
    public async Task<IEnumerable<ListEntry>> GetListEntriesAsync(
        int? listId = null, 
        string? productId = null, 
        int? storeId = null, 
        int? quantity = null)
    {
        var query = context.ListEntry.AsQueryable();
    
        if (listId != null)
        {
            query = query.Where(pil => pil.ListId == listId);
        }
    
        if (!string.IsNullOrEmpty(productId))
        {
            query = query.Where(pil => EF.Functions.TrigramsSimilarity(pil.ProductId, productId) > SimilarityThreshold);
        }
    
        if (storeId != null)
        {
            query = query.Where(pil => pil.StoreId == storeId);
        }
    
        if (quantity != null)
        {
            query = query.Where(pil => pil.Quantity == quantity);
        }
    
        return await query
            .Select(pilEntity => pilEntity.ToListEntry())
            .ToListAsync();
    }

    public async Task<ListEntry?> GetListEntriesByListIdAsync(int listId, string productId)
    {
        var productInListEntity = await context.ListEntry.FindAsync(listId, productId);
        return productInListEntity?.ToListEntry();
    }

    public async Task<int> AddListEntryAsync(int listId, string productId, int storeId, int quantity)
    {
        var productInListEntity = new ListEntryEntity()
        {
            ListId = listId,
            ProductId = productId,
            StoreId = storeId,
            Quantity = quantity
        };
        
        await context.ListEntry.AddAsync(productInListEntity);
        await context.SaveChangesAsync();
        
        return productInListEntity.ListId;
    }

    public async Task<ListEntry?> UpdateListEntryAsync(int listId, string productId, int? storeId = null, int? quantity = null)
    {
        var productInListEntity = await context.ListEntry.FindAsync(listId, productId);
        
        if (productInListEntity == null)
        {
            return null;
        }
        
        if (quantity != null)
        {
            productInListEntity.Quantity = quantity.Value;
        }
        
        if (storeId != null)
        {
            productInListEntity.StoreId = storeId.Value;
        }
        
        await context.SaveChangesAsync();
        
        return productInListEntity.ToListEntry();
    }

    public async Task<ListEntry?> DeleteListEntryAsync(int listId, string productId)
    {
        var productInListEntity = await context.ListEntry.FindAsync(listId, productId);
        
        if (productInListEntity == null)
        {
            return null;
        }
        
        context.ListEntry.Remove(productInListEntity);
        await context.SaveChangesAsync();
        
        return productInListEntity.ToListEntry();
    }
}