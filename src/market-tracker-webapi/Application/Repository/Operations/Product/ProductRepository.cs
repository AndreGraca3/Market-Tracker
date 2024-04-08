using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.Product;

using Product = Domain.Product;

public class ProductRepository(MarketTrackerDataContext dataContext) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await dataContext
            .Product.Select(productEntity => productEntity.ToProduct())
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(string productId)
    {
        var productEntity = await dataContext.Product.FindAsync(productId);
        return productEntity?.ToProduct();
    }

    public async Task<string> AddProductAsync(
        string productId,
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
            Id = productId,
            Name = name,
            ImageUrl = imageUrl,
            Quantity = quantity,
            Unit = unit,
            BrandId = brandId,
            CategoryId = categoryId
        };
        await dataContext.Product.AddAsync(productEntity);
        await dataContext.SaveChangesAsync();
        return productEntity.Id;
    }

    public async Task<Product?> UpdateProductAsync(
        string productId,
        string? name,
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

        productEntity.Name = name ?? productEntity.Name;
        productEntity.ImageUrl = imageUrl ?? productEntity.ImageUrl;
        productEntity.Quantity = quantity ?? productEntity.Quantity;
        productEntity.Unit = unit ?? productEntity.Unit;
        productEntity.BrandId = brandId ?? productEntity.BrandId;
        productEntity.CategoryId = categoryId ?? productEntity.CategoryId;

        await dataContext.SaveChangesAsync();
        return productEntity.ToProduct();
    }

    public async Task<Product?> RemoveProductAsync(string productId)
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
}
