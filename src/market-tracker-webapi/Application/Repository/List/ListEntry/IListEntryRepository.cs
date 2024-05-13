using market_tracker_webapi.Application.Domain.Schemas.List;

namespace market_tracker_webapi.Application.Repository.List.ListEntry;

using ListEntry = Domain.Schemas.List.ListEntry;

public interface IListEntryRepository
{
    Task<IEnumerable<ListEntry>> GetListEntriesAsync(string listId, int? storeId = null);

    Task<ListEntry?> GetListEntryAsync(string listId, string productId);

    Task<ListEntryId> AddListEntryAsync(string listId, string productId, int storeId, int quantity);

    Task<ListEntry?> UpdateListEntryAsync(string listId, string productId, int? storeId = null, int? quantity = null);

    Task<ListEntry?> DeleteListEntryAsync(string listId, string productId);
}