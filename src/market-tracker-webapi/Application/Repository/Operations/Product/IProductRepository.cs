using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Product;

namespace market_tracker_webapi.Application.Repository.Operations.Product;

using Product = Domain.Product;

public interface IProductRepository
{
    Task<PaginatedResult<ProductInfo>> GetAvailableProductsAsync(
        int skip,
        int take,
        SortByType? sortBy = null,
        string? name = null,
        IList<int>? categoryIds = null,
        IList<int>? brandIds = null,
        int? minRating = null,
        int? maxRating = null
    );

    Task<ProductDetails?> GetProductByIdAsync(string productId);

    Task<string> AddProductAsync(
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