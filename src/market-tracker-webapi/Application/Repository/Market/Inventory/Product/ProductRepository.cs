using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Filters.Product;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory.Product;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Market.Inventory.Product;

using Product = Domain.Models.Market.Inventory.Product.Product;

public class ProductRepository(MarketTrackerDataContext dataContext) : IProductRepository
{
    private const double SimilarityThreshold = 0.1;

    public async Task<PaginatedResult<Product>> GetAvailableProductsAsync(
        int skip,
        int take,
        ProductsSortOption? sortBy,
        string? name,
        IList<int>? categoryIds,
        IList<int>? brandIds,
        int? minRating,
        int? maxRating
    )
    {
        var query = from product in dataContext.Product
            orderby EF.Functions.TrigramsSimilarity(product.Name, name) descending
            join brand in dataContext.Brand on product.BrandId equals brand.Id
            join category in dataContext.Category on product.CategoryId equals category.Id
            join availability in dataContext.ProductAvailability on product.Id equals availability.ProductId
            where availability.IsAvailable &&
                  (categoryIds == null || categoryIds.Contains(product.CategoryId)) &&
                  (brandIds == null || brandIds.Contains(product.BrandId)) &&
                  (minRating == null || product.Rating >= minRating) &&
                  (maxRating == null || product.Rating <= maxRating) &&
                  (name == null || EF.Functions.TrigramsSimilarity(product.Name, name) > SimilarityThreshold)
            select new
            {
                Product = product,
                Brand = brand,
                Category = category
            };

        query = sortBy switch
        {
            ProductsSortOption.Popularity => query.OrderBy(queryRes => queryRes.Product.Views),
            ProductsSortOption.NameLowToHigh => query.OrderBy(queryRes => queryRes.Product.Name),
            ProductsSortOption.NameHighToLow => query.OrderByDescending(queryRes => queryRes.Product.Name),
            ProductsSortOption.RatingLowToHigh => query.OrderBy(queryRes => queryRes.Product.Rating),
            ProductsSortOption.RatingHighToLow => query.OrderByDescending(queryRes => queryRes.Product.Rating),
            _ => query
        };

        var items = await query
            .Skip(skip)
            .Take(take)
            .Select(queryRes =>
                queryRes.Product.ToProduct(queryRes.Brand.ToBrand(), queryRes.Category.ToCategory())
            )
            .ToListAsync();

        return new PaginatedResult<Product>(items, query.Count(), skip, take);
    }

    public async Task<Product?> GetProductByIdAsync(string productId)
    {
        var query =
            from product in dataContext.Product
            join brand in dataContext.Brand on product.BrandId equals brand.Id
            join category in dataContext.Category on product.CategoryId equals category.Id
            where product.Id == productId
            select new
            {
                ProductEntity = product,
                BrandEntity = brand,
                CategoryEntity = category
            };

        return await query.Select(
            g => g.ProductEntity.ToProduct(g.BrandEntity.ToBrand(), g.CategoryEntity.ToCategory())
        ).FirstOrDefaultAsync();
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

    public async Task SetProductAvailabilityAsync(
        string productId,
        int storeId,
        bool isAvailable
    )
    {
        var availability = await dataContext.ProductAvailability
            .Where(availability => availability.ProductId == productId && availability.StoreId == storeId)
            .FirstOrDefaultAsync();

        if (availability is null)
        {
            availability = new ProductAvailabilityEntity()
            {
                ProductId = productId,
                StoreId = storeId,
                IsAvailable = isAvailable
            };
            await dataContext.ProductAvailability.AddAsync(availability);
        }
        else
        {
            availability.IsAvailable = isAvailable;
        }

        await dataContext.SaveChangesAsync();
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
        return await GetProductByIdAsync(productId);
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
        return await GetProductByIdAsync(productId);
    }
}