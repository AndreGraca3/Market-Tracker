namespace market_tracker_webapi.Application.Repository.List.ListEntry;

public interface IListEntryRepository
{
    Task<IEnumerable<Domain.Models.List.ListEntry>> GetListEntriesAsync(int listId, int? storeId = null);
    
    Task<Domain.Models.List.ListEntry?> GetListEntryAsync(int listId, string productId);
    
    Task<int> AddListEntryAsync(int listId, string productId, int storeId, int quantity);
    
    Task<Domain.Models.List.ListEntry?> UpdateListEntryAsync(int listId, string productId, int? storeId = null, int? quantity = null);
    
    Task<Domain.Models.List.ListEntry?> DeleteListEntryAsync(int listId, string productId);
}