using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.Product;

using Product = Domain.Product;

public class ProductRepository(MarketTrackerDataContext dataContext) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await dataContext.Product
            .Select(productEntity => productEntity.ToProduct())
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int productId)
    {
        var productEntity = await dataContext.Product.FindAsync(productId);
        return productEntity?.ToProduct();
    }

    public async Task<int> AddProductAsync(
        int id,
        string name,
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
            ImageUrl = imageUrl,
            Quantity = quantity,
            Unit = unit,
            Views = 0,
            Rate = 0,
            BrandId = brandId,
            CategoryId = categoryId
        };
        await dataContext.Product.AddAsync(productEntity);
        await dataContext.SaveChangesAsync();
        return productEntity.Id;
    }

    public async Task<Product?> UpdateProductAsync(
        int productId,
        string? imageUrl = null,
        int? quantity = null,
        string? unit = null,
        int? brandId = null,
        int? categoryId = null
    )
    {
        var productEntity = await dataContext.Product.FindAsync(productId);
        if (productEntity is null)
        {
            return null;
        }

        productEntity.ImageUrl = imageUrl ?? productEntity.ImageUrl;
        productEntity.Quantity = quantity ?? productEntity.Quantity;
        productEntity.Unit = unit ?? productEntity.Unit;
        productEntity.BrandId = brandId ?? productEntity.BrandId;
        productEntity.CategoryId = categoryId ?? productEntity.CategoryId;

        await dataContext.SaveChangesAsync();
        return productEntity.ToProduct();
    }

    public async Task<Product?> RemoveProductAsync(int productId)
    {
        var productEntity = await dataContext.Product.FindAsync(productId);
        if (productEntity is null)
        {
            return null;
        }

        dataContext.Product.Remove(productEntity);
        await dataContext.SaveChangesAsync();
        return productEntity.ToProduct();
    }

    public async Task<IEnumerable<ProductReview>> GetReviewsByProductIdAsync(Guid clientId, int productId)
    {
        return await dataContext.ProductReview
            .Where(review => review.ProductId == productId)
            .OrderByDescending(review => review.ClientId == clientId) // Client's review first
            .ThenByDescending(review => review.CreatedAt)
            .Select(productReviewEntity => productReviewEntity.ToProductReview())
            .ToListAsync();
    }

    public async Task AddReviewAsync(Guid clientId, int productId, int rate, string comment)
    {
        var reviewEntity = new ProductReviewEntity()
        {
            ClientId = clientId,
            ProductId = productId,
            Rate = rate,
            Comment = comment,
            CreatedAt = DateTime.Now
        };
        dataContext.ProductReview.Add(reviewEntity);
        await dataContext.SaveChangesAsync();
    }

    public async Task<ProductReview?> UpdateReviewAsync(
        Guid clientId,
        int productId,
        int rate,
        string comment
    )
    {
        var reviewEntity = await dataContext.ProductReview.FindAsync(clientId, productId);
        if (reviewEntity is null)
        {
            return null;
        }

        reviewEntity.Rate = rate;
        reviewEntity.Comment = comment;

        await dataContext.SaveChangesAsync();
        return reviewEntity.ToProductReview();
    }

    public async Task<ProductReview?> RemoveReviewAsync(Guid clientId, int productId)
    {
        var reviewEntity = await dataContext.ProductReview.FindAsync(clientId, productId);
        if (reviewEntity is null)
        {
            return null;
        }

        dataContext.ProductReview.Remove(reviewEntity);

        await dataContext.SaveChangesAsync();
        return reviewEntity.ToProductReview();
    }
}