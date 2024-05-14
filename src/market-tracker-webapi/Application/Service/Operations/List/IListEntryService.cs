using market_tracker_webapi.Application.Domain.Filters.List;
using market_tracker_webapi.Application.Domain.Schemas.List;
using market_tracker_webapi.Application.Service.Results;

namespace market_tracker_webapi.Application.Service.Operations.List;

public interface IListEntryService
{
    Task<ShoppingListEntriesResult> GetListEntriesAsync(string listId, Guid clientId,
        ShoppingListAlternativeType? alternativeType,
        IList<int>? companyIds,
        IList<int>? storeIds,
        IList<int>? cityIds);

    Task<ListEntryId> AddListEntryAsync(string listId, Guid clientId, string productId,
        int storeId, int quantity);

    Task<ListEntry> UpdateListEntryAsync(string listId, Guid clientId, string entryId,
        int? storeId, int? quantity);

    Task<ListEntryId> DeleteListEntryAsync(string listId, Guid clientId, string entryId);
}