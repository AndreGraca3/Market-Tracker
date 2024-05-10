using market_tracker_webapi.Application.Domain.Models.List;
using market_tracker_webapi.Application.Http.Models.Schemas.List;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Results;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.List;

public interface IListService
{
    Task<Either<IServiceError, IEnumerable<ShoppingList>>> GetListsAsync(Guid clientId, bool isOwner,
        string? listName, DateTime? createdAfter, bool? isArchived);

    Task<Either<ListFetchingError, ShoppingListResult>> GetListByIdAsync(int id, Guid clientId);

    Task<Either<IServiceError, ShoppingListId>> AddListAsync(Guid clientId, string listName);

    Task<Either<IServiceError, ShoppingList>> UpdateListAsync(int id, Guid clientId, string? listName,
        bool? isArchived);

    Task<Either<ListFetchingError, ShoppingList>> DeleteListAsync(int id, Guid clientId);

    Task<Either<IServiceError, ListClient>> AddClientToListAsync(int listId, Guid clientId, Guid clientIdToAdd);

    Task<Either<IListError, ListClient>> RemoveClientFromListAsync(int listId, Guid clientId,
        Guid clientIdToRemove);
}