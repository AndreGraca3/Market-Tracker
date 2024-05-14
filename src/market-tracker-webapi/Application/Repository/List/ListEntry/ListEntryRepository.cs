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
            join store in dataContext.Store on listEntry.StoreId equals store.Id
            where listEntry.ListId == listId && (storeId == null || listEntry.StoreId == storeId)
            select new
            {
                ListEntryEntity = listEntry,
                ProductEntity = product,
                StoreEntity = store,
                BrandEntity = brand,
                CategoryEntity = category
            };

        return await query
            .Select(g =>
                g.ListEntryEntity.ToListEntry(
                    g.ProductEntity.ToProduct(g.BrandEntity.ToBrand(), g.CategoryEntity.ToCategory()),
                    g.StoreEntity.ToStoreItem()
                )
            )
            .ToListAsync();
    }

    public async Task<ListEntry?> GetListEntryByIdAsync(string entryId)
    {
        var query = from listEntry in dataContext.ListEntry
            join product in dataContext.Product on listEntry.ProductId equals product.Id
            join brand in dataContext.Brand on product.BrandId equals brand.Id
            join category in dataContext.Category on product.CategoryId equals category.Id
            join store in dataContext.Store on listEntry.StoreId equals store.Id
            where listEntry.Id == entryId
            select new
            {
                ListEntryEntity = listEntry,
                ProductEntity = product,
                StoreEntity = store,
                BrandEntity = brand,
                CategoryEntity = category
            };

        return await query
            .Select(g => g.ListEntryEntity.ToListEntry(
                g.ProductEntity.ToProduct(g.BrandEntity.ToBrand(), g.CategoryEntity.ToCategory()),
                g.StoreEntity.ToStoreItem())
            ).FirstOrDefaultAsync();
    }

    public async Task<ListEntry?> GetListEntryByProductIdAsync(string listId, string productId)
    {
        var query = from listEntry in dataContext.ListEntry
            where listEntry.ListId == listId && listEntry.ProductId == productId
            select listEntry
            into listEntryEntity
            join product in dataContext.Product on listEntryEntity.ProductId equals product.Id
            join brand in dataContext.Brand on product.BrandId equals brand.Id
            join category in dataContext.Category on product.CategoryId equals category.Id
            join store in dataContext.Store on listEntryEntity.StoreId equals store.Id
            select new
            {
                ListEntryEntity = listEntryEntity,
                ProductEntity = product,
                StoreEntity = store,
                BrandEntity = brand,
                CategoryEntity = category
            };
        return await query
            .Select(g =>
                g.ListEntryEntity.ToListEntry(
                    g.ProductEntity.ToProduct(g.BrandEntity.ToBrand(), g.CategoryEntity.ToCategory()),
                    g.StoreEntity.ToStoreItem()
                )
            )
            .FirstOrDefaultAsync();
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

    public async Task<ListEntry?> UpdateListEntryByIdAsync(string entryId, int? storeId,
        int? quantity)
    {
        var listEntryEntity = await dataContext.ListEntry.FindAsync(entryId);

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

        return await GetListEntryByIdAsync(entryId);
    }

    public async Task<ListEntry?> DeleteListEntryByIdAsync(string entryId)
    {
        var productInListEntity = await dataContext.ListEntry.FindAsync(entryId);

        if (productInListEntity == null)
        {
            return null;
        }

        dataContext.ListEntry.Remove(productInListEntity);
        await dataContext.SaveChangesAsync();

        return await GetListEntryByIdAsync(entryId);
    }
}