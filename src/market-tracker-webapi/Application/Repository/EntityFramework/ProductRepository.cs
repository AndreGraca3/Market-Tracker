using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository.Interfaces;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.EntityFramework;

public class ProductRepository(MarketTrackerDataContext dataContext) : IProductRepository
{
    public async Task<List<Product>> GetProductsAsync()
    {
        return await dataContext
            .Product.Select(product => new Product(
                product.Id,
                product.Name,
                product.Description,
                product.ImageUrl,
                product.Quantity,
                product.Unit,
                product.Views,
                product.Rate,
                product.BrandId,
                product.CategoryId
            ))
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int productId)
    {
        var productEntity = await dataContext.Product.FindAsync(productId);
        return productEntity is null
            ? null
            : new Product(
                productEntity.Id,
                productEntity.Name,
                productEntity.Description,
                productEntity.ImageUrl,
                productEntity.Quantity,
                productEntity.Unit,
                productEntity.Views,
                productEntity.Rate,
                productEntity.BrandId,
                productEntity.CategoryId
            );
    }

    public async Task<int> AddProductAsync(
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
        await dataContext.Product.AddAsync(productEntity);
        await dataContext.SaveChangesAsync();
        return productEntity.Id;
    }

    public async Task<Product> UpdateProductAsync(
        int productId,
        float? price = null,
        string? description = null,
        string? imageUrl = null,
        int? quantity = null,
        string? unit = null,
        int? brandId = null,
        int? categoryId = null,
        int? rate = null
    )
    {
        throw new NotImplementedException();
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
        return new Product(
            productEntity.Id,
            productEntity.Name,
            productEntity.Description,
            productEntity.ImageUrl,
            productEntity.Quantity,
            productEntity.Unit,
            productEntity.Views,
            productEntity.Rate,
            productEntity.BrandId,
            productEntity.CategoryId
        );
    }

    public async Task<List<ProductReview>> GetReviewsByProductIdAsync(int productId)
    {
        var reviews = await dataContext
            .ProductReview.Where(review => review.ProductId == productId)
            .ToListAsync();
        return reviews
            .Select(review => new ProductReview(
                review.ClientId,
                review.ProductId,
                review.Rate,
                review.Comment,
                review.CreatedAt
            ))
            .ToList();
    }

    public async Task<int> AddReviewAsync(Guid clientId, int productId, int rate, string comment)
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
        return await dataContext.SaveChangesAsync();
    }

    public Task<ProductReview> UpdateReviewAsync(
        Guid clientId,
        int productId,
        int rate,
        string comment
    )
    {
        throw new NotImplementedException();
    }

    public Task RemoveReviewAsync(Guid clientId, int productId)
    {
        throw new NotImplementedException();
    }
}
