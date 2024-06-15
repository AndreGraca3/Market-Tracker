using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Filters.Product;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory.Product;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Market.Inventory.Product;

using Product = Domain.Schemas.Market.Inventory.Product.Product;

public class ProductRepository(MarketTrackerDataContext dataContext) : IProductRepository
{
    private const double SimilarityThreshold = 0.9;

    public async Task<PaginatedFacetedProducts> GetAvailableProductsAsync(int skip, int take, int maxValuesPerFacet,
        ProductsSortOption? sortBy = null, string? name = null, IList<int>? categoryIds = null,
        IList<int>? brandIds = null, int? minPrice = null, int? maxPrice = null, int? minRating = null,
        int? maxRating = null, IList<int>? companyIds = null, IList<int>? storeIds = null, IList<int>? cityIds = null)
    {
        var baseQuery = from product in dataContext.Product
            join availability in dataContext.ProductAvailability on product.Id equals availability.ProductId
            join store in dataContext.Store on availability.StoreId equals store.Id
            where availability.IsAvailable
            join price in dataContext.PriceEntry on product.Id equals price.ProductId
            where price.StoreId == store.Id &&
                  price.CreatedAt == dataContext.PriceEntry
                      .Where(pe => pe.ProductId == product.Id && pe.StoreId == store.Id)
                      .Max(pe => pe.CreatedAt)
            join brand in dataContext.Brand on product.BrandId equals brand.Id
            join category in dataContext.Category on product.CategoryId equals category.Id
            join company in dataContext.Company on store.CompanyId equals company.Id
            join city in dataContext.City on store.CityId equals city.Id into cityGroup
            from city in cityGroup.DefaultIfEmpty()
            join promotion in dataContext.Promotion on price.Id equals promotion.PriceEntryId into promotionGroup
            from promotion in promotionGroup.DefaultIfEmpty()
            where (name == null || EF.Functions.ILike(product.Name, $"%{name}%") ||
                   EF.Functions.TrigramsSimilarityDistance(product.Name, name) < SimilarityThreshold)
                  && (categoryIds == null || categoryIds.Contains(product.CategoryId))
                  && (brandIds == null || brandIds.Contains(product.BrandId))
                  && (companyIds == null || companyIds.Contains(company.Id))
                  && (storeIds == null || storeIds.Contains(store.Id))
                  && (cityIds == null || cityIds.Contains(city.Id))
                  && (minRating == null || product.Rating >= minRating)
                  && (maxRating == null || product.Rating <= maxRating)
                  && (minPrice == null ||
                      price.Price - (promotion == null ? 0 : price.Price * (promotion.Percentage / 100)) >=
                      minPrice)
                  && (maxPrice == null ||
                      price.Price - (promotion == null ? 0 : price.Price * (promotion.Percentage / 100)) <=
                      maxPrice)
            select new
            {
                Product = product,
                Brand = brand,
                Category = category,
                Store = store,
                Company = company,
                City = city,
                Price = price,
                Promotion = promotion,
                Availability = availability
            };

        // Facets
        var categoryFacets = await baseQuery
            .Where(p =>
                (brandIds == null || brandIds.Contains(p.Product.BrandId)) &&
                (companyIds == null || companyIds.Contains(p.Company.Id))
            )
            .GroupBy(p => p.Product.CategoryId)
            .Select(grouping => new FacetCounter
            {
                Id = grouping.Key,
                Name = grouping.First().Category.Name,
                Count = grouping.GroupBy(g => g.Product.Id).Count()
            })
            .OrderByDescending(facet => facet.Count).ToListAsync();

        var brandFacets = await baseQuery
            .Where(p =>
                (categoryIds == null || categoryIds.Contains(p.Product.CategoryId)) &&
                (companyIds == null || companyIds.Contains(p.Company.Id))
            )
            .GroupBy(p => p.Product.BrandId)
            .Select(grouping => new FacetCounter
            {
                Id = grouping.Key,
                Name = grouping.First().Brand.Name,
                Count = grouping.GroupBy(g => g.Product.Id).Count()
            }).OrderByDescending(facet => facet.Count).Take(maxValuesPerFacet).ToListAsync();

        var companyFacets = await baseQuery
            .Where(p =>
                (categoryIds == null || categoryIds.Contains(p.Product.CategoryId)) &&
                (brandIds == null || brandIds.Contains(p.Product.BrandId))
            )
            .GroupBy(p => p.Company.Id)
            .Select(grouping => new FacetCounter
            {
                Id = grouping.Key,
                Name = grouping.First().Company.Name,
                Count = grouping.GroupBy(g => g.Product.Id).Count()
            })
            .OrderByDescending(facet => facet.Count)
            .ToListAsync();

        var facets = new ProductsFacetsCounters(brandFacets, categoryFacets, companyFacets);

        var query = baseQuery.GroupBy(p => p.Product.Id);

        var orderedQuery = sortBy switch
        {
            ProductsSortOption.Popularity => query.OrderByDescending(queryRes => queryRes.First().Product.Views),
            ProductsSortOption.NameLowToHigh => query.OrderBy(queryRes => queryRes.First().Product.Name),
            ProductsSortOption.NameHighToLow => query.OrderByDescending(queryRes => queryRes.First().Product.Name),
            ProductsSortOption.RatingLowToHigh => query.OrderBy(queryRes => queryRes.First().Product.Rating),
            ProductsSortOption.RatingHighToLow => query.OrderByDescending(queryRes => queryRes.First().Product.Rating),
            ProductsSortOption.PriceLowToHigh => query.OrderBy(queryRes =>
                queryRes.First().Price.Price - (queryRes.First().Promotion == null
                    ? 0
                    : queryRes.First().Price.Price * (queryRes.First().Promotion.Percentage / 100))),
            ProductsSortOption.PriceHighToLow => query.OrderByDescending(queryRes =>
                queryRes.First().Price.Price - (queryRes.First().Promotion == null
                    ? 0
                    : queryRes.First().Price.Price * (queryRes.First().Promotion.Percentage / 100))),
            _ => query.OrderBy(queryRes =>
                    name == null ? 1 : EF.Functions.TrigramsSimilarityDistance(queryRes.First().Product.Name, name))
                .ThenBy(queryRes => queryRes.First().Product.Name)
        };

        var products = await orderedQuery
            .Skip(skip)
            .Take(take)
            .Select(g =>
                g.First().Product.ToProduct(g.First().Brand.ToBrand(), g.First().Category.ToCategory()))
            .ToListAsync();

        return new PaginatedFacetedProducts(
            new PaginatedResult<Product>(products, query.Count(), skip, take),
            facets
        );
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

    public async Task<ProductId> AddProductAsync(
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
        return new ProductId(productEntity.Id);
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
            availability = new ProductAvailabilityEntity
            {
                ProductId = productId,
                StoreId = storeId,
                IsAvailable = isAvailable,
                LastChecked = DateTime.Now
            };
            await dataContext.ProductAvailability.AddAsync(availability);
        }
        else
        {
            availability.IsAvailable = isAvailable;
            availability.LastChecked = DateTime.Now;
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

        var product = await GetProductByIdAsync(productId);
        dataContext.Product.Remove(productEntity);
        await dataContext.SaveChangesAsync();
        return product;
    }
}