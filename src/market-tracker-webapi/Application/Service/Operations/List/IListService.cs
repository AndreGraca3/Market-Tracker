using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository.Dto.List;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Errors.ListEntry;
using market_tracker_webapi.Application.Utils;
using ListEntry = market_tracker_webapi.Application.Domain.ListEntry;

namespace market_tracker_webapi.Application.Service.Operations.List;

public interface IListService
{
    Task<Either<IServiceError, CollectionOutputModel>> GetListsAsync(Guid clientId, string? listName,
        DateTime? createdAfter, bool? isArchived, bool? isOwner
    );
    
    Task<Either<ListFetchingError, ListProduct>> GetListByIdAsync(int id, Guid clientId);

    Task<Either<IServiceError, IntIdOutputModel>> AddListAsync(Guid clientId, string listName);

    Task<Either<IServiceError, ListOfProducts>> UpdateListAsync(int id, Guid clientId, string? listName,
        bool? isArchived
    );

    Task<Either<ListFetchingError, ListOfProducts>> DeleteListAsync(int id, Guid clientId);
    
    Task<Either<IServiceError, ListClient>> AddClientToListAsync(int listId, Guid clientIdToAdd, Guid clientId);
    
    Task<Either<IServiceError, ListClient>> RemoveClientFromListAsync(int listId, Guid clientId);
}