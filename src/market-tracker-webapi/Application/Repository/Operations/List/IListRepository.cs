using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Operations.List;

public interface IListRepository
{
    Task<IEnumerable<ListOfProducts>> GetListsAsync(Guid clientId, string? listName = null,
        DateTime? createdAfter = null, bool? isArchived = null, bool? isOwner = null
    );

    Task<IEnumerable<Guid>> GetListClientsByListIdAsync(int  listId);
    
    Task<ListOfProducts?> GetListByIdAsync(int id);

    Task<int> AddListAsync(string listName, Guid ownerId);

    Task<ListOfProducts?> UpdateListAsync(int id, DateTime? archivedAt, string? listName = null);

    Task<ListOfProducts?> DeleteListAsync(int id);
    
    Task<ListClient> AddListClientAsync(int listId, Guid clientId);
    
    Task<ListClient?> DeleteListClientAsync(int listId, Guid clientId);
}