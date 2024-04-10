using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository.Dto.List;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Errors.ListEntry;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.List;

public interface IListService
{
    Task<CollectionOutputModel> GetListsAsync(Guid clientId, string? listName, DateTime? archivedAt);
    
    Task<Either<ListFetchingError, ListProduct>> GetListByIdAsync(int id);
    
    Task<Either<IServiceError, IntIdOutputModel>> AddListAsync(Guid clientId, string listName);
    
    Task<Either<IServiceError, ListOfProducts>> UpdateListAsync(int id, Guid clientId, string? listName, DateTime? archivedAt);
    
    Task<Either<ListFetchingError, ListOfProducts>> DeleteListAsync(int id);
    
    Task<CollectionOutputModel> GetListEntriesAsync(int? listId, string? productId, int? storeId, int? quantity);
    
    Task<Either<IServiceError, ListEntry>> GetListEntryByIdAsync(int listId, string productId, int storeId);
    
    Task<Either<IServiceError, IntIdOutputModel>> AddListEntryAsync(int listId, string productId, int storeId, int quantity);
    
    Task<Either<IServiceError, ProductInList>> UpdateListEntryAsync(int listId, string productId, int storeId, int? quantity = null);
    
    Task<Either<ListEntryFetchingError, ProductInList>> DeleteListEntryAsync(int listId, string productId, int storeId);
}