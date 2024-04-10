using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Operations.List;


public interface IListRepository
{
    Task<IEnumerable<ListOfProducts>> GetListsAsync(Guid clientId, string? listName = null, DateTime? archivedAt = null, DateTime? createdAt = null);
    
    Task<ListOfProducts?> GetListByIdAsync(int id);
    
    Task<int> AddListAsync(Guid clientId, string listName);
    
    Task<ListOfProducts?> UpdateListAsync(int id, string? listName = null, DateTime? archivedAt = null);
    
    Task<ListOfProducts?> DeleteListAsync(int id);
    
    Task<IEnumerable<ProductInList>> GetProductsInListAsync(int? listId = null, string? productId = null, int? storeId = null, int? quantity = null);
    
    Task<ProductInList?> GetProductsByListIdAsync(int listId, string productId, int storeId);
    
    Task<int> AddProductInListAsync(int listId, string productId, int storeId, int quantity);
    
    Task<ProductInList?> UpdateProductInListAsync(int listId, string productId, int storeId, int? quantity = null);
    
    Task<ProductInList?> DeleteProductInListAsync(int listId, string productId, int storeId);
}