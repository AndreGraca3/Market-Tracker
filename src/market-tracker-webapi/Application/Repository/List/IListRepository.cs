using market_tracker_webapi.Application.Domain.Models.Account.Users;
using market_tracker_webapi.Application.Domain.Models.List;

namespace market_tracker_webapi.Application.Repository.List;

public interface IListRepository
{
    Task<IEnumerable<ShoppingList>> GetListsAsync(Guid clientId, bool isOwner, string? listName = null,
        DateTime? createdAfter = null, bool? isArchived = null
    );

    Task<IEnumerable<UserId>> GetClientIdsByListIdAsync(int  listId);
    
    Task<bool> IsClientInListAsync(int listId, Guid clientId);
    
    Task<ShoppingList?> GetListByIdAsync(int id);

    Task<ShoppingListId> AddListAsync(string listName, Guid ownerId);

    Task<ShoppingList?> UpdateListAsync(int id, DateTime? archivedAt, string? listName = null);

    Task<ShoppingList?> DeleteListAsync(int id);
    
    Task<ListClient> AddListClientAsync(int listId, Guid clientId);
    
    Task<ListClient?> DeleteListClientAsync(int listId, Guid clientId);
}