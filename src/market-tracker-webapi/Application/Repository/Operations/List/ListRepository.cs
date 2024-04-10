using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.List;

public class ListRepository(MarketTrackerDataContext context) : IListRepository
{
    public async Task<IEnumerable<ListOfProducts>> GetListsAsync(Guid clientId, string? listName = null, DateTime? archivedAt = null, DateTime? createdAt = null)
    {
        var query = context.List.AsQueryable()
            .Where(l => l.ClientId == clientId);
        
        if (!string.IsNullOrEmpty(listName))
        {
            query = query.Where(l => EF.Functions.Like(l.Name, $"%{listName}%"));
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

    public async Task<IEnumerable<ProductInList>> GetProductsInListAsync(int? listId = null, string? productId = null, int? storeId = null, int? quantity = null)
    {
        var query = context.ListEntry.AsQueryable();
        
        if (listId != null)
        {
            query = query.Where(pil => pil.ListId == listId);
        }
        
        if (productId != null)
        {
            query = query.Where(pil => pil.ProductId == productId);
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
            .Select(pilEntity => pilEntity.ToProductInList())
            .ToListAsync();
    }

    public async Task<ProductInList?> GetProductsByListIdAsync(int listId, string productId, int storeId)
    {
        var productInListEntity = await context.ListEntry.FindAsync(listId, productId, storeId);
        return productInListEntity?.ToProductInList();
    }

    public async Task<int> AddProductInListAsync(int listId, string productId, int storeId, int quantity)
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

    public async Task<ProductInList?> UpdateProductInListAsync(int listId, string productId, int storeId, int? quantity = null)
    {
        var productInListEntity = await context.ListEntry.FindAsync(listId, productId, storeId);
        
        if (productInListEntity == null)
        {
            return null;
        }
        
        if (quantity != null)
        {
            productInListEntity.Quantity = quantity.Value;
        }
        
        await context.SaveChangesAsync();
        
        return productInListEntity.ToProductInList();
    }

    public async Task<ProductInList?> DeleteProductInListAsync(int listId, string productId, int storeId)
    {
        var productInListEntity = await context.ListEntry.FindAsync(listId, productId, storeId);
        
        if (productInListEntity == null)
        {
            return null;
        }
        
        context.ListEntry.Remove(productInListEntity);
        await context.SaveChangesAsync();
        
        return productInListEntity.ToProductInList();
    }
}