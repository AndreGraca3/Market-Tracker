using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Operations.Product;

using Product = Domain.Product;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync();

    Task<Product?> GetProductByIdAsync(int productId);

    Task<int> AddProductAsync(
        int id,
        string name,
        string imageUrl,
        int quantity,
        string unit,
        int brandId,
        int categoryId
    );

    Task<Product?> UpdateProductAsync(
        int productId,
        string? imageUrl = null,
        int? quantity = null,
        string? unit = null,
        int? brandId = null,
        int? categoryId = null
    );

    Task<Product?> RemoveProductAsync(int productId);

    Task<IEnumerable<ProductReview>> GetReviewsByProductIdAsync(Guid clientId, int productId);

    Task AddReviewAsync(Guid clientId, int productId, int rate, string comment);

    Task<ProductReview?> UpdateReviewAsync(Guid clientId, int productId, int rate, string comment);

    Task<ProductReview?> RemoveReviewAsync(Guid clientId, int productId);
}
