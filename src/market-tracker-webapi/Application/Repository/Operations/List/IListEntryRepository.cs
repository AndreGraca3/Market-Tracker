using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Operations.List;

public interface IListEntryRepository
{
    Task<IEnumerable<ListEntry>> GetListEntriesAsync(int? listId = null, string? productId = null, int? storeId = null, int? quantity = null);
    
    Task<ListEntry?> GetListEntriesByListIdAsync(int listId, string productId);
    
    Task<int> AddListEntryAsync(int listId, string productId, int storeId, int quantity);
    
    Task<ListEntry?> UpdateListEntryAsync(int listId, string productId, int? storeId = null, int? quantity = null);
    
    Task<ListEntry?> DeleteListEntryAsync(int listId, string productId);
}