using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Filters.Product;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;

namespace market_tracker_webapi.Application.Repository.Market.Inventory.Product;

using Product = Domain.Schemas.Market.Inventory.Product.Product;

public interface IProductRepository
{
    /**
     * Get available products. If any of the filters is null, it will not be applied.
     * If no sort option is provided, Relevance will be used.
     */
    Task<PaginatedResult<Product>> GetAvailableProductsAsync(
        int skip, int take, ProductsSortOption? sortBy = null, string? name = null, IList<int>? categoryIds = null,
        IList<int>? brandIds = null, int? minPrice = null, int? maxPrice = null, int? minRating = null,
        int? maxRating = null, IList<int>? companyIds = null, IList<int>? storeIds = null, IList<int>? cityIds = null
    );

    /**
     * Get the facets with counts for the available products.
     * If any of the filters is null, it will not be applied.
     */
    Task<ProductsFacetsCounters> GetProductsFacetsCountersAsync(
        int maxValuesPerFacet, string? name = null, int? minPrice = null, int? maxPrice = null, int? minRating = null,
        int? maxRating = null, IList<int>? categoryIds = null, IList<int>? brandIds = null,
        IList<int>? companyIds = null);

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