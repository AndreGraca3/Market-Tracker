using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Domain.Schemas.List;

namespace market_tracker_webapi.Application.Repository.List;

public interface IListRepository
{
    Task<IEnumerable<ShoppingList>> GetListsFromClientAsync(Guid clientId, bool? isOwner = null, string? listName = null,
        DateTime? createdAfter = null, bool? isArchived = null
    );

    Task<IEnumerable<ClientItem>> GetClientMembersByListIdAsync(string  listId);
    
    Task<ShoppingList?> GetListByIdAsync(string id);

    Task<ShoppingListId> AddListAsync(string listName, Guid ownerId);

    Task<ShoppingListItem?> UpdateListAsync(string id, DateTime? archivedAt, string? listName = null);

    Task<ShoppingListItem?> DeleteListAsync(string id);
    
    Task<ListClient> AddListMemberAsync(string listId, Guid clientId);
    
    Task<ListClient?> DeleteListMemberAsync(string listId, Guid clientId);
}