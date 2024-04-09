using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.List;

public class ListRepository(MarketTrackerDataContext context) : IListRepository
{
    public async Task<IEnumerable<ListOfProducts>> GetListsOfProductsAsync(Guid clientId, string? listName = null, DateTime? archivedAt = null, DateTime? createdAt = null)
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

    public async Task<ListOfProducts?> GetListOfProductsByIdAsync(int id)
    {
        var listEntity = await context.List.FindAsync(id);
        return listEntity?.ToListOfProducts();
    }

    public async Task<int> AddListOfProductsAsync(Guid clientId, string listName)
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

    public async Task<ListOfProducts?> UpdateListOfProductsAsync(int id, string? listName = null, DateTime? archivedAt = null)
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

    public async Task<ListOfProducts?> DeleteListOfProductsAsync(int id)
    {
        var listEntity = await context.List.FindAsync(id);
        
        if (listEntity == null)
        {
            return null;
        }
        
        var productInListEntities = await context.ProductInList
            .Where(pil => pil.ListId == id)
            .ToListAsync();
        
        context.ProductInList.RemoveRange(productInListEntities);
        context.List.Remove(listEntity);
        await context.SaveChangesAsync();
        
        return listEntity.ToListOfProducts();
    }

    public async Task<IEnumerable<ProductInList>> GetProductsInListAsync(int? listId = null, int? productId = null, int? storeId = null, int? quantity = null)
    {
        var query = context.ProductInList.AsQueryable();
        
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

    public async Task<ProductInList?> GetProductsByListIdAsync(int listId, int productId, int storeId)
    {
        var productInListEntity = await context.ProductInList.FindAsync(listId, productId, storeId);
        return productInListEntity?.ToProductInList();
    }

    public async Task<int> AddProductInListAsync(int listId, int productId, int storeId, int quantity)
    {
        var productInListEntity = new ProductInListEntity()
        {
            ListId = listId,
            ProductId = productId,
            StoreId = storeId,
            Quantity = quantity
        };
        
        await context.ProductInList.AddAsync(productInListEntity);
        await context.SaveChangesAsync();
        
        return productInListEntity.ListId;
    }

    public async Task<ProductInList?> UpdateProductInListAsync(int listId, int productId, int storeId, int? quantity = null)
    {
        var productInListEntity = await context.ProductInList.FindAsync(listId, productId, storeId);
        
        if (productInListEntity == null || quantity == null)
        {
            return null;
        }
        
        await context.SaveChangesAsync();
        
        return productInListEntity.ToProductInList();
    }

    public async Task<ProductInList?> DeleteProductInListAsync(int listId, int productId, int storeId)
    {
        var productInListEntity = await context.ProductInList.FindAsync(listId, productId, storeId);
        
        if (productInListEntity == null)
        {
            return null;
        }
        
        context.ProductInList.Remove(productInListEntity);
        await context.SaveChangesAsync();
        
        return productInListEntity.ToProductInList();
    }
}