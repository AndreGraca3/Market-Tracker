using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Filters.Product;
using market_tracker_webapi.Application.Domain.Models.Market.Inventory.Product;

namespace market_tracker_webapi.Application.Repository.Market.Inventory.Product;

using Product = Domain.Models.Market.Inventory.Product.Product;

public interface IProductRepository
{
    Task<PaginatedResult<Product>> GetAvailableProductsAsync(
        int skip,
        int take,
        ProductsSortOption? sortBy = null,
        string? name = null,
        IList<int>? categoryIds = null,
        IList<int>? brandIds = null,
        int? minRating = null,
        int? maxRating = null
    );

    Task<Product?> GetProductByIdAsync(string productId);

    Task<ProductId> AddProductAsync(
        string productId,
        string name,
        string imageUrl,
        int quantity,
        string unit,
        int brandId,
        int categoryId
    );
    
    public Task SetProductAvailabilityAsync(
        string productId,
        int storeId,
        bool isAvailable
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