using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository.Interfaces;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;

namespace market_tracker_webapi.Application.Repository.EntityFramework;

public class ProductRepository(MarketTrackerDataContext marketTrackerDataContext)
    : IProductRepository
{
    public async Task<ProductEntity?> GetProductAsync(int productId)
    {
        return await marketTrackerDataContext.Product.FindAsync(productId);
    }

    public Task<int> AddProductAsync(
        int id,
        string name,
        string description,
        string imageUrl,
        int quantity,
        string unit,
        int brandId,
        int categoryId
    )
    {
        var productEntity = new ProductEntity()
        {
            Id = id,
            Name = name,
            Description = description,
            ImageUrl = imageUrl,
            Quantity = quantity,
            Unit = unit,
            Views = 0,
            Rate = 0,
            BrandId = brandId,
            CategoryId = categoryId
        };
        marketTrackerDataContext.Product.Add(productEntity);
        return Task.FromResult(productEntity.Id);
    }

    public Task<IdOutputModel> UpdateProductAsync(
        int productId,
        float? price = null,
        string? description = null
    )
    {
        throw new NotImplementedException();
    }

    public Task<ProductEntity> DeleteProductAsync(ProductEntity product)
    {
        marketTrackerDataContext.Product.Remove(product);
        return Task.FromResult(product);
    }

    public Task<List<IdOutputModel>> GetProductsAsync(
        string filter,
        Pagination pagination
    )
    {
        throw new NotImplementedException();
    }

    public Task<BrandModel> GetBrandAsync(int brandId)
    {
        throw new NotImplementedException();
    }

    public Task<int> AddBrandAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task DeleteBrandAsync(int brandId)
    {
        throw new NotImplementedException();
    }

    public Task<List<CommentModel>> GetReviewFromProduct(int commentId)
    {
        throw new NotImplementedException();
    }

    public Task<int> CreateReviewAsync(int productId, int userId, CommentModel commentModel)
    {
        throw new NotImplementedException();
    }

    public Task<CommentModel> UpdateReviewAsync(int commentId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteReviewAsync(int commentId)
    {
        throw new NotImplementedException();
    }
}
