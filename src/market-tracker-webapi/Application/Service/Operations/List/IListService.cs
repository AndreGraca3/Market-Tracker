using market_tracker_webapi.Application.Domain.Schemas.List;
using market_tracker_webapi.Application.Service.Results;

namespace market_tracker_webapi.Application.Service.Operations.List;

public interface IListService
{
    Task<IEnumerable<ShoppingList>> GetListsAsync(Guid clientId, bool? isOwner,
        string? listName, DateTime? createdAfter, bool? isArchived);

    Task<ShoppingListResult> GetListByIdAsync(string listId, Guid clientId);

    Task<ShoppingListId> AddListAsync(Guid clientId, string listName);

    Task<ShoppingListItem> UpdateListAsync(string listId, Guid clientId, string? listName, bool? isArchived);

    Task<ShoppingListItem> DeleteListAsync(string listId, Guid clientId);

    Task<ListClient> AddClientToListAsync(string listId, Guid clientId, Guid clientIdToAdd);

    Task<ListClient> RemoveClientFromListAsync(string listId, Guid clientId,Guid clientIdToRemove);
}