using market_tracker_webapi.Application.Domain.Filters.Product;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Service.Results;

namespace market_tracker_webapi.Application.Service.Operations.Market.Inventory.Product;

using Product = Domain.Schemas.Market.Inventory.Product.Product;

public interface IProductService
{
    public Task<PaginatedProductOffers> GetBestAvailableProductsOffersAsync(
        int skip,
        int take,
        int maxValuesPerFacet,
        ProductsSortOption? sortBy,
        string? searchName,
        IList<int>? categoryIds,
        IList<int>? brandIds,
        IList<int>? companyIds,
        int? minPrice,
        int? maxPrice,
        int? minRating,
        int? maxRating
    );

    public Task<Product> GetProductByIdAsync(string productId);

    public Task<ProductCreationResult> AddProductAsync(
        Guid operatorId,
        string productId,
        string name,
        string imageUrl,
        int quantity,
        ProductUnit unit,
        string brandName,
        int categoryId,
        int price,
        int? promotionPercentage
    );

    public Task<ProductId> SetProductAvailabilityAsync(
        Guid operatorId,
        string productId,
        bool isAvailable
    );

    public Task<Product> UpdateProductAsync(
        string productId,
        string? name,
        string? imageUrl,
        int? quantity,
        string? unit,
        string? brandName,
        int? categoryId
    );

    public Task<ProductId> RemoveProductAsync(string productId);
}