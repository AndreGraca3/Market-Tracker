using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Operations.Product;

using Product = Domain.Product;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync();

    Task<Product?> GetProductByIdAsync(string productId);

    Task<string> AddProductAsync(
        string productId,
        string name,
        string imageUrl,
        int quantity,
        string unit,
        int brandId,
        int categoryId
    );

    Task<Product?> UpdateProductAsync(
        string productId,
        string? name = null,
        string? imageUrl = null,
        int? quantity = null,
        string? unit = null,
        int? brandId = null,
        int? categoryId = null
    );

    Task<Product?> RemoveProductAsync(string productId);
}
