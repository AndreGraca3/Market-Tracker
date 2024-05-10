using market_tracker_webapi.Application.Domain.Filters.Product;
using market_tracker_webapi.Application.Domain.Models.Market.Inventory.Product;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.Inventory.Product;

using Product = Domain.Models.Market.Inventory.Product.Product;

public interface IProductService
{
    public Task<Either<IServiceError, PaginatedProductOffers>> GetBestAvailableProductsOffersAsync(
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

    public Task<Either<ProductFetchingError, Product>> GetProductByIdAsync(string productId);

    public Task<Either<IServiceError, ProductCreationOutputModel>> AddProductAsync(
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
    
    public Task<Either<IServiceError, StringIdOutputModel>> SetProductAvailabilityAsync(
        Guid operatorId,
        string productId,
        bool isAvailable
    );

    public Task<Either<IServiceError, ProductInfoOutputModel>> UpdateProductAsync(
        string productId,
        string? name,
        string? imageUrl,
        int? quantity,
        string? unit,
        string? brandName,
        int? categoryId
    );

    public Task<Either<ProductFetchingError, StringIdOutputModel>> RemoveProductAsync(
        string productId
    );
}