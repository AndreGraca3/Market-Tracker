using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository.Dto.List;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.ListEntry;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.List;

public interface IListEntryService
{
    Task<Either<IServiceError, ListEntryDetails>> GetListEntryByIdAsync(int listId, string productId);
    
    Task<Either<IServiceError, IntIdOutputModel>> AddListEntryAsync(int listId, string productId, int storeId, int quantity);
    
    Task<Either<IServiceError, ListEntry>> UpdateListEntryAsync(int listId, string productId, int? storeId, int? quantity);
    
    Task<Either<ListEntryFetchingError, ListEntry>> DeleteListEntryAsync(int listId, string productId);
}