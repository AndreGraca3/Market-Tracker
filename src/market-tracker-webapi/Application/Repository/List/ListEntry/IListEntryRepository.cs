using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Domain.Models.List;

namespace market_tracker_webapi.Application.Repository.Operations.List;

public interface IListEntryRepository
{
    Task<IEnumerable<ListEntry>> GetListEntriesAsync(int listId, int? storeId = null);
    
    Task<ListEntry?> GetListEntryAsync(int listId, string productId);
    
    Task<int> AddListEntryAsync(int listId, string productId, int storeId, int quantity);
    
    Task<ListEntry?> UpdateListEntryAsync(int listId, string productId, int? storeId = null, int? quantity = null);
    
    Task<ListEntry?> DeleteListEntryAsync(int listId, string productId);
}