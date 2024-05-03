using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Price;
using market_tracker_webapi.Application.Repository.Dto.Product;
using market_tracker_webapi.Application.Repository.Dto.Store;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.Price;

public class PriceRepository(MarketTrackerDataContext dataContext) : IPriceRepository
{
    private const double SimilarityThreshold = 0.1;

    public async Task<PaginatedProductOffers> GetBestAvailableProductsOffersAsync(int skip, int take,
        SortByType? sortBy = null, string? name = null, IList<int>? categoryIds = null, IList<int>? brandIds = null,
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
                Promotion = promotion
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
            SortByType.Popularity => query.OrderBy(queryRes => queryRes.Product.Views),
            SortByType.NameLowToHigh => query.OrderBy(queryRes => queryRes.Product.Name),
            SortByType.NameHighToLow => query.OrderByDescending(queryRes => queryRes.Product.Name),
            SortByType.RatingLowToHigh => query.OrderBy(queryRes => queryRes.Product.Rating),
            SortByType.RatingHighToLow => query.OrderByDescending(queryRes => queryRes.Product.Rating),
            SortByType.PriceLowToHigh => query.OrderBy(queryRes =>
                queryRes.Price.Price - (queryRes.Promotion == null
                    ? 0
                    : queryRes.Price.Price * queryRes.Promotion.Percentage / 100)),
            SortByType.PriceHighToLow => query.OrderByDescending(queryRes =>
                queryRes.Price.Price - (queryRes.Promotion == null
                    ? 0
                    : queryRes.Price.Price * queryRes.Promotion.Percentage / 100)),
            _ => query
        };

        var bestOffers = orderedQuery
            .GroupBy(g => g.Product.Id)
            .Select(group =>
                group.Select(g => new ProductOffer(
                    ProductInfo.ToProductInfo(g.Product.ToProduct(), g.Brand.ToBrand(), g.Category.ToCategory()),
                    new StorePrice(
                        StoreInfo.ToStoreInfo(g.Store.ToStore(), g.City == null ? null : g.City.ToCity(),
                            g.Company.ToCompany()),
                        PriceInfo.Calculate(g.Price.Price,
                            g.Promotion == null ? null : g.Promotion.ToPromotion(g.Price.Price),
                            g.Price.CreatedAt))
                ))
            )
            .Skip(skip)
            .Take(take)
            .ToList()
            .Select(group => group.MinBy(g => g.StorePrice!.PriceData.FinalPrice)!);

        bestOffers = sortBy switch
        {
            SortByType.Popularity => bestOffers.OrderBy(queryRes => queryRes.Product.Id).ToList(),
            SortByType.NameLowToHigh => bestOffers.OrderBy(queryRes => queryRes.Product.Name).ToList(),
            SortByType.NameHighToLow => bestOffers.OrderByDescending(queryRes => queryRes.Product.Name).ToList(),
            SortByType.RatingLowToHigh => bestOffers.OrderBy(queryRes => queryRes.Product.Rating).ToList(),
            SortByType.RatingHighToLow => bestOffers.OrderByDescending(queryRes => queryRes.Product.Rating).ToList(),
            SortByType.PriceLowToHigh => bestOffers.OrderBy(queryRes => queryRes.StorePrice!.PriceData.FinalPrice)
                .ToList(),
            SortByType.PriceHighToLow => bestOffers
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
        var query = await (
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
            }
        ).ToListAsync();

        if (query.Count == 0)
        {
            return null;
        }

        var cheapestStore = query
            .Select(group =>
                new StorePrice(StoreInfo.ToStoreInfo(group.Store.ToStore(), group.City?.ToCity(),
                    group.Company.ToCompany()), PriceInfo.Calculate(group.PriceEntry.Price,
                    group.Promotion?.ToPromotion(group.PriceEntry.Price), group.PriceEntry.CreatedAt))
            ).MinBy(group => group.PriceData.FinalPrice);

        return cheapestStore;
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

    public async Task<StoreAvailability?> GetStoreAvailabilityAsync(string productId, int storeId)
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
        var queryRes = await (
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
            }
        ).FirstOrDefaultAsync();

        if (queryRes is null)
        {
            return null;
        }

        return new StorePrice(
            StoreInfo.ToStoreInfo(
                queryRes.Store.ToStore(),
                queryRes.City?.ToCity(),
                queryRes.Company.ToCompany()
            ),
            PriceInfo.Calculate(
                queryRes.PriceEntry.Price,
                queryRes.Promotion?.ToPromotion(queryRes.PriceEntry.Price),
                queryRes.PriceEntry.CreatedAt
            )
        );
    }

    public async Task<IEnumerable<PriceInfo>> GetPriceHistoryByProductIdAndStoreIdAsync(
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
            .Select(res => PriceInfo.Calculate(
                res.PriceEntry.Price,
                res.Promotion.ToPromotion(res.PriceEntry.Price),
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