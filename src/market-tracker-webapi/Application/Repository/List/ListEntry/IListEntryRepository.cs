using market_tracker_webapi.Application.Domain.Models.List;

namespace market_tracker_webapi.Application.Repository.List.ListEntry;

using ListEntry = Domain.Models.List.ListEntry;

public interface IListEntryRepository
{
    Task<IEnumerable<ListEntry>> GetListEntriesAsync(int listId, int? storeId = null);

    Task<ListEntry?> GetListEntryAsync(int listId, string productId);

    Task<ListEntryId> AddListEntryAsync(int listId, string productId, int storeId, int quantity);

    Task<ListEntry?> UpdateListEntryAsync(int listId, string productId, int? storeId = null, int? quantity = null);

    Task<ListEntry?> DeleteListEntryAsync(int listId, string productId);
}