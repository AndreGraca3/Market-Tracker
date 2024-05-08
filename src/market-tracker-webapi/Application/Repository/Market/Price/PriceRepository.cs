using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Filters.Product;
using market_tracker_webapi.Application.Domain.Models.Market.Inventory.Product;
using market_tracker_webapi.Application.Domain.Models.Market.Retail.Sales;
using market_tracker_webapi.Application.Domain.Models.Market.Store;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Market.Price;

using Price = Domain.Models.Market.Retail.Sales.Pricing.Price;

public class PriceRepository(MarketTrackerDataContext dataContext) : IPriceRepository
{
    private const double SimilarityThreshold = 0.1;

    public async Task<PaginatedProductOffers> GetBestAvailableProductsOffersAsync(int skip, int take,
        ProductsSortOption? sortBy = null, string? name = null, IList<int>? categoryIds = null,
        IList<int>? brandIds = null,
        int? minPrice = null,
        int? maxPrice = null, int? minRating = null, int? maxRating = null, IList<int>? companyIds = null,
        IList<int>? storeIds = null, IList<int>? cityIds = null)
    {
        var query = from product in dataContext.Product
            join price in dataContext.PriceEntry on product.Id equals price.ProductId
            join brand in dataContext.Brand on product.BrandId equals brand.Id
            join category in dataContext.Category on product.CategoryId equals category.Id
            join store in dataContext.Store on price.StoreId equals store.Id
            join availability in dataContext.ProductAvailability on store.Id equals availability.StoreId
            where availability.ProductId == product.Id
            join company in dataContext.Company on store.CompanyId equals company.Id
            join city in dataContext.City on store.CityId equals city.Id into cityGroup
            from city in cityGroup.DefaultIfEmpty()
            join promotion in dataContext.Promotion on price.Id equals promotion.PriceEntryId into promotionGroup
            from promotion in promotionGroup.DefaultIfEmpty()
            where price.CreatedAt == dataContext.PriceEntry
                      .Where(pe => pe.ProductId == product.Id && pe.StoreId == store.Id)
                      .Max(pe => pe.CreatedAt)
                  && availability.IsAvailable
                  && (categoryIds == null || categoryIds.Contains(product.CategoryId))
                  && (brandIds == null || brandIds.Contains(product.BrandId))
                  && (minRating == null || product.Rating >= minRating)
                  && (maxRating == null || product.Rating <= maxRating)
                  && (name == null || EF.Functions.ILike(product.Name, $"%{name}%") ||
                      EF.Functions.TrigramsWordSimilarity(product.Name, name) > SimilarityThreshold)
                  && (companyIds == null || companyIds.Contains(company.Id))
                  && (storeIds == null || storeIds.Contains(store.Id))
                  && (cityIds == null || cityIds.Contains(city.Id))
                  && (minPrice == null ||
                      price.Price - (promotion == null ? 0 : price.Price * promotion.Percentage / 100) >= minPrice)
                  && (maxPrice == null ||
                      price.Price - (promotion == null ? 0 : price.Price * promotion.Percentage / 100) <= maxPrice)
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

        var categoryFacets = await query
            .GroupBy(p => p.Product.CategoryId)
            .Select(grouping => new FacetCounter
            {
                Id = grouping.Key,
                Name = grouping.First().Category.Name,
                Count = grouping.GroupBy(g => g.Product.Id).Count()
            }).ToListAsync();

        var brandFacets = await query
            .GroupBy(p => p.Product.BrandId)
            .Select(grouping => new FacetCounter
            {
                Id = grouping.Key,
                Name = grouping.First().Brand.Name,
                Count = grouping.GroupBy(g => g.Product.Id).Count()
            }).ToListAsync();

        var companyFacets = await query
            .GroupBy(p => p.Company.Id)
            .Select(grouping => new FacetCounter
            {
                Id = grouping.Key,
                Name = grouping.First().Company.Name,
                Count = grouping.GroupBy(g => g.Product.Id).Count()
            }).ToListAsync();

        var orderedQuery = sortBy switch
        {
            ProductsSortOption.Popularity => query.OrderBy(queryRes => queryRes.Product.Views),
            ProductsSortOption.NameLowToHigh => query.OrderBy(queryRes => queryRes.Product.Name),
            ProductsSortOption.NameHighToLow => query.OrderByDescending(queryRes => queryRes.Product.Name),
            ProductsSortOption.RatingLowToHigh => query.OrderBy(queryRes => queryRes.Product.Rating),
            ProductsSortOption.RatingHighToLow => query.OrderByDescending(queryRes => queryRes.Product.Rating),
            ProductsSortOption.PriceLowToHigh => query.OrderBy(queryRes =>
                queryRes.Price.Price - (queryRes.Promotion == null
                    ? 0
                    : queryRes.Price.Price * queryRes.Promotion.Percentage / 100)),
            ProductsSortOption.PriceHighToLow => query.OrderByDescending(queryRes =>
                queryRes.Price.Price - (queryRes.Promotion == null
                    ? 0
                    : queryRes.Price.Price * queryRes.Promotion.Percentage / 100)),
            _ => query
        };

        var bestOffers = orderedQuery
            .GroupBy(g => g.Product.Id)
            .Select(group =>
                group.Select(g => new ProductOffer(
                    g.Product.ToProduct(g.Brand.ToBrand(), g.Category.ToCategory()),
                    new StorePrice(
                        g.Store.ToStore(
                            g.City == null ? null : g.City.ToCity(),
                            g.Company.ToCompany()
                        ),
                        new Price(g.Price.Price,
                            g.Promotion == null ? null : g.Promotion.ToPromotion(),
                            g.Price.CreatedAt)
                    ), g.Availability.IsAvailable
                ))
            )
            .Skip(skip)
            .Take(take)
            .ToList()
            .Select(group => group.MinBy(g => g.StorePrice!.PriceData.FinalPrice)!);

        bestOffers = sortBy switch
        {
            ProductsSortOption.Popularity => bestOffers.OrderBy(queryRes => queryRes.Product.Id).ToList(),
            ProductsSortOption.NameLowToHigh => bestOffers.OrderBy(queryRes => queryRes.Product.Name).ToList(),
            ProductsSortOption.NameHighToLow =>
                bestOffers.OrderByDescending(queryRes => queryRes.Product.Name).ToList(),
            ProductsSortOption.RatingLowToHigh => bestOffers.OrderBy(queryRes => queryRes.Product.Rating).ToList(),
            ProductsSortOption.RatingHighToLow => bestOffers.OrderByDescending(queryRes => queryRes.Product.Rating)
                .ToList(),
            ProductsSortOption.PriceLowToHigh => bestOffers
                .OrderBy(queryRes => queryRes.StorePrice!.PriceData.FinalPrice)
                .ToList(),
            ProductsSortOption.PriceHighToLow => bestOffers
                .OrderByDescending(queryRes => queryRes.StorePrice!.PriceData.FinalPrice)
                .ToList(),
            _ => bestOffers
        };

        var productsFacetsCounters = new ProductsFacetsCounters(brandFacets, categoryFacets, companyFacets);

        var paginatedProducts = new PaginatedResult<ProductOffer>(bestOffers,
            query.GroupBy(g => g.Product.Id).Count(), skip, take);

        return new PaginatedProductOffers(paginatedProducts, productsFacetsCounters);
    }

    public async Task<StorePrice?> GetCheapestStorePriceAvailableByProductIdAsync(
        string productId,
        IList<int>? companyIds,
        IList<int>? storeIds,
        IList<int>? cityIds
    )
    {
        var query =
            from priceEntry in dataContext.PriceEntry
            where priceEntry.CreatedAt == dataContext.PriceEntry
                .Where(pe => pe.ProductId == productId && pe.StoreId == priceEntry.StoreId)
                .Max(pe => pe.CreatedAt)
            join store in dataContext.Store on priceEntry.StoreId equals store.Id
            join company in dataContext.Company on store.CompanyId equals company.Id
            join city in dataContext.City on store.CityId equals city.Id into cityGroup
            from city in cityGroup.DefaultIfEmpty()
            join promotion in dataContext.Promotion
                on priceEntry.Id equals promotion.PriceEntryId
                into promotionGroup
            from promotion in promotionGroup.DefaultIfEmpty()
            join availability in dataContext.ProductAvailability on store.Id equals availability.StoreId
            where availability.ProductId == productId && availability.IsAvailable
            where companyIds == null || companyIds.Contains(company.Id)
            where storeIds == null || storeIds.Contains(store.Id)
            where cityIds == null || cityIds.Contains(city.Id)
            select new
            {
                Store = store,
                City = city,
                Company = company,
                PriceEntry = priceEntry,
                Promotion = promotion
            };

        if (!query.Any())
        {
            return null;
        }

        return query
            .Select(g =>
                new StorePrice(
                    g.Store.ToStore(g.City == null ? null : g.City.ToCity(), g.Company.ToCompany()),
                    new Price(
                        g.PriceEntry.Price,
                        g.Promotion == null ? null : g.Promotion.ToPromotion(),
                        g.PriceEntry.CreatedAt)
                )
            ).MinBy(group => group.PriceData.FinalPrice);
    }

    public async Task<IEnumerable<StoreAvailability>> GetStoresAvailabilityAsync(string productId)
    {
        var query = dataContext.ProductAvailability.AsQueryable();

        return await query
            .Where(availability => availability.ProductId == productId)
            .Join(
                dataContext.Company,
                availability => availability.StoreId,
                company => company.Id,
                (availability, company) => new { availability, company }
            )
            .Select(availabilityTuple => new StoreAvailability(
                availabilityTuple.availability.StoreId,
                availabilityTuple.availability.ProductId,
                availabilityTuple.company.Id,
                availabilityTuple.availability.IsAvailable,
                availabilityTuple.availability.LastChecked
            ))
            .ToListAsync();
    }

    public async Task<StoreAvailability?> GetStoreAvailabilityStatusAsync(string productId, int storeId)
    {
        var queryRes = await (
            from availability in dataContext.ProductAvailability
            where availability.ProductId == productId && availability.StoreId == storeId
            join company in dataContext.Company on storeId equals company.Id
            select new { availability, company }
        ).FirstOrDefaultAsync();

        if (queryRes is null)
        {
            return null;
        }

        return new StoreAvailability(
            queryRes.availability.StoreId,
            queryRes.availability.ProductId,
            queryRes.company.Id,
            queryRes.availability.IsAvailable,
            queryRes.availability.LastChecked
        );
    }

    public async Task<StorePrice?> GetStorePriceAsync(
        string productId,
        int storeId,
        DateTime priceAt
    )
    {
        var query =
            from priceEntry in dataContext.PriceEntry
            where
                priceEntry.ProductId == productId
                && priceEntry.StoreId == storeId
                && priceEntry.CreatedAt <= priceAt
            orderby priceEntry.CreatedAt descending
            join store in dataContext.Store on priceEntry.StoreId equals store.Id
            join company in dataContext.Company on store.CompanyId equals company.Id
            join city in dataContext.City on store.CityId equals city.Id into cityGroup
            from city in cityGroup.DefaultIfEmpty()
            join promotion in dataContext.Promotion
                on priceEntry.Id equals promotion.PriceEntryId
                into promotionGroup
            from promotion in promotionGroup.DefaultIfEmpty()
            select new
            {
                Store = store,
                City = city,
                Company = company,
                PriceEntry = priceEntry,
                Promotion = promotion
            };

        return await query
            .Select(g =>
                new StorePrice(
                    g.Store.ToStore(g.City == null ? null : g.City.ToCity(), g.Company.ToCompany()),
                    new Price(
                        g.PriceEntry.Price,
                        g.Promotion == null ? null : g.Promotion.ToPromotion(),
                        g.PriceEntry.CreatedAt)
                )
            ).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Price>>
        GetPriceHistoryByProductIdAndStoreIdAsync(
            string productId,
            int storeId,
            DateTime pricedAfter,
            DateTime pricedBefore
        )
    {
        var query = (
            from priceEntry in dataContext.PriceEntry
            where
                priceEntry.ProductId == productId
                && priceEntry.StoreId == storeId
                && priceEntry.CreatedAt >= pricedAfter
                && priceEntry.CreatedAt <= pricedBefore
            join promotion in dataContext.Promotion
                on priceEntry.Id equals promotion.PriceEntryId
                into promotionGroup
            from promotion in promotionGroup.DefaultIfEmpty()
            select new { PriceEntry = priceEntry, Promotion = promotion }
        );

        return await query
            .Select(res => new Price(
                res.PriceEntry.Price,
                res.Promotion.ToPromotion(),
                res.PriceEntry.CreatedAt
            ))
            .ToListAsync();
    }

    public async Task<string> AddPriceAsync(
        string productId,
        int storeId,
        int price,
        int? promotionPercentage
    )
    {
        var priceEntry = new PriceEntryEntity
        {
            ProductId = productId,
            StoreId = storeId,
            Price = price
        };

        await dataContext.PriceEntry.AddAsync(priceEntry);

        await dataContext.SaveChangesAsync();

        if (promotionPercentage.HasValue)
        {
            var promotion = new PromotionEntity
            {
                Percentage = promotionPercentage.Value,
                PriceEntryId = priceEntry.Id
            };

            await dataContext.Promotion.AddAsync(promotion);
        }

        await dataContext.SaveChangesAsync();

        return priceEntry.Id;
    }
}