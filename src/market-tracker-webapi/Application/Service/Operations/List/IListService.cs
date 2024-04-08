using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Errors.ProductInList;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.List;

public interface IListService
{
    Task<CollectionOutputModel> GetListsOfProductsAsync(Guid? clientId, string? listName, DateTime? archivedAt);
    
    Task<Either<ListFetchingError, ListOfProducts>> GetListOfProductsByIdAsync(int id);
    
    Task<Either<IServiceError, int>> AddListOfProductsAsync(Guid clientId, string listName);
    
    Task<Either<IServiceError, ListOfProducts>> UpdateListOfProductsAsync(int id, Guid clientId, string? listName, DateTime? archivedAt);
    
    Task<Either<ListFetchingError, ListOfProducts>> DeleteListOfProductsAsync(int id);
    
    Task<CollectionOutputModel> GetProductsInListAsync(int? listId, int? productId, int? storeId, int? quantity);
    
    Task<Either<IServiceError, ProductInList>> GetProductsByListIdAsync(int listId, int productId, int storeId);
    
    Task<Either<IServiceError, int>> AddProductInListAsync(int listId, int productId, int storeId, int quantity);
    
    Task<Either<IServiceError, ProductInList>> UpdateProductInListAsync(int listId, int productId, int storeId, int? quantity = null);
    
    Task<Either<IServiceError, ProductInList>> DeleteProductInListAsync(int listId, int productId, int storeId);
}