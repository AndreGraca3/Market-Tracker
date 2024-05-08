using market_tracker_webapi.Application.Repository.Operations.List;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.List;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.List.ListEntry;

using ListEntry = Domain.Models.List.ListEntry;

public class ListEntryRepository(MarketTrackerDataContext dataContext) : IListEntryRepository
{
    public async Task<IEnumerable<ListEntry>> GetListEntriesAsync(int listId, int? storeId = null)
    {
        var query = from listEntries in dataContext.ListEntry
            where listEntries.ListId == listId && (storeId == null || listEntries.StoreId == storeId)
            select new
            {
                ListEntryEntity = listEntries
            };


        return await query
            .Select(g => g.ListEntryEntity.ToListEntry())
            .ToListAsync();
    }

    public async Task<ListEntry?> GetListEntryAsync(int listId, string productId)
    {
        var productInListEntity = await dataContext.ListEntry.FindAsync(listId, productId);
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

        await dataContext.ListEntry.AddAsync(productInListEntity);
        await dataContext.SaveChangesAsync();

        return productInListEntity.ListId;
    }

    public async Task<ListEntry?> UpdateListEntryAsync(int listId, string productId,
        int? storeId = null,
        int? quantity = null)
    {
        var productInListEntity = await dataContext.ListEntry.FindAsync(listId, productId);

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

        await dataContext.SaveChangesAsync();

        return productInListEntity.ToListEntry();
    }

    public async Task<ListEntry?> DeleteListEntryAsync(int listId, string productId)
    {
        var productInListEntity = await dataContext.ListEntry.FindAsync(listId, productId);

        if (productInListEntity == null)
        {
            return null;
        }

        dataContext.ListEntry.Remove(productInListEntity);
        await dataContext.SaveChangesAsync();

        return productInListEntity.ToListEntry();
    }
}