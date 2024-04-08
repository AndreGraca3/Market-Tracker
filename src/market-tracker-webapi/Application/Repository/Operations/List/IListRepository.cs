using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Operations.List;


public interface IListRepository
{
    Task<IEnumerable<ListOfProducts>> GetListsOfProductsAsync(Guid? clientId = null, string? listName = null, DateTime? archivedAt = null);
    
    Task<ListOfProducts?> GetListOfProductsByIdAsync(int id);
    
    Task<int> AddListOfProductsAsync(Guid clientId, string listName);
    
    Task<ListOfProducts?> UpdateListOfProductsAsync(int id, string? listName = null, DateTime? archivedAt = null);
    
    Task<ListOfProducts?> DeleteListOfProductsAsync(int id);
    
    Task<IEnumerable<ProductInList>> GetProductsInListAsync(int? listId = null, int? productId = null, int? storeId = null, int? quantity = null);
    
    Task<ProductInList?> GetProductsByListIdAsync(int listId, int productId, int storeId);
    
    Task<int> AddProductInListAsync(int listId, int productId, int storeId, int quantity);
    
    Task<ProductInList?> UpdateProductInListAsync(int listId, int productId, int storeId, int? quantity = null);
    
    Task<ProductInList?> DeleteProductInListAsync(int listId, int productId, int storeId);
}