using market_tracker_webapi.Application.Domain.Schemas.List;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.List;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.List.ListEntry;

using ListEntry = Domain.Schemas.List.ListEntry;

public class ListEntryRepository(MarketTrackerDataContext dataContext) : IListEntryRepository
{
    public async Task<IEnumerable<ListEntry>> GetListEntriesAsync(string listId, int? storeId = null)
    {
        var query = from listEntry in dataContext.ListEntry
            join product in dataContext.Product on listEntry.ProductId equals product.Id
            join brand in dataContext.Brand on product.BrandId equals brand.Id
            join category in dataContext.Category on product.CategoryId equals category.Id
            where listEntry.ListId == listId && (storeId == null || listEntry.StoreId == storeId)
            select new
            {
                ListEntryEntity = listEntry,
                ProductEntity = product,
                BrandEntity = brand,
                CategoryEntity = category
            };

        return await query
            .Select(g =>
                g.ListEntryEntity.ToListEntry(g.ProductEntity.ToProduct(g.BrandEntity.ToBrand(),
                    g.CategoryEntity.ToCategory())))
            .ToListAsync();
    }

    public async Task<ListEntry?> GetListEntryAsync(string listId, string productId)
    {
        var query = from listEntry in dataContext.ListEntry
            join product in dataContext.Product on listEntry.ProductId equals product.Id
            join brand in dataContext.Brand on product.BrandId equals brand.Id
            join category in dataContext.Category on product.CategoryId equals category.Id
            where listEntry.ListId == listId && listEntry.ProductId == productId
            select new
            {
                ListEntryEntity = listEntry,
                ProductEntity = product,
                BrandEntity = brand,
                CategoryEntity = category
            };

        var result = await query.FirstOrDefaultAsync();

        return result?.ListEntryEntity.ToListEntry(result.ProductEntity.ToProduct(result.BrandEntity.ToBrand(),
            result.CategoryEntity.ToCategory()));
    }

    public async Task<ListEntryId> AddListEntryAsync(string listId, string productId, int storeId, int quantity)
    {
        var productInListEntity = new ListEntryEntity()
        {
            ListId = listId,
            ProductId = productId,
            StoreId = storeId,
            Quantity = quantity
        };

        await dataContext.ListEntry.AddAsync(productInListEntity);
        await dataContext.SaveChangesAsync();

        return new ListEntryId(productInListEntity.Id);
    }

    public async Task<ListEntry?> UpdateListEntryAsync(string listId, string productId, int? storeId = null,
        int? quantity = null)
    {
        var listEntryEntity = await dataContext.ListEntry.FindAsync(listId, productId);

        if (listEntryEntity == null)
        {
            return null;
        }

        if (quantity != null)
        {
            listEntryEntity.Quantity = quantity.Value;
        }

        if (storeId != null)
        {
            listEntryEntity.StoreId = storeId.Value;
        }

        await dataContext.SaveChangesAsync();

        return await GetListEntryAsync(listId, productId);
    }

    public async Task<ListEntry?> DeleteListEntryAsync(string listId, string productId)
    {
        var productInListEntity = await dataContext.ListEntry.FindAsync(listId, productId);

        if (productInListEntity == null)
        {
            return null;
        }

        dataContext.ListEntry.Remove(productInListEntity);
        await dataContext.SaveChangesAsync();

        return await GetListEntryAsync(listId, productId);
    }
}