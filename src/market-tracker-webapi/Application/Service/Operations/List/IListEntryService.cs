using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Domain.Models.List;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Http.Models.Schemas.List.ListEntry;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Errors.ListEntry;
using market_tracker_webapi.Application.Service.Results;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.List;

public interface IListEntryService
{
    Task<Either<ListFetchingError, ShoppingListEntriesResult>> GetListEntriesAsync(int listId, Guid clientId,
        ShoppingListAlternativeType? alternativeType,
        IList<int>? companyIds,
        IList<int>? storeIds,
        IList<int>? cityIds);

    Task<Either<IServiceError, ListEntryId>> AddListEntryAsync(int listId, Guid clientId, string productId,
        int storeId, int quantity);

    Task<Either<IServiceError, ListEntry>> UpdateListEntryAsync(int listId, Guid clientId, string productId,
        int? storeId, int? quantity);

    Task<Either<IServiceError, ListEntryId>> DeleteListEntryAsync(int listId, Guid clientId, string productId);
}