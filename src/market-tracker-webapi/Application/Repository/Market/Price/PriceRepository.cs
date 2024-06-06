using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Filters.Product;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales.Pricing;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Market.Price;

using Price = Domain.Schemas.Market.Retail.Sales.Pricing.Price;

public class PriceRepository(MarketTrackerDataContext dataContext) : IPriceRepository
{
    private const double SimilarityThreshold = 0.1;

    public async Task<PaginatedProductOffers> GetBestAvailableProductsOffersAsync(int skip, int take,
        int maxValuesPerFacet, ProductsSortOption? sortBy = null, string? name = null, IList<int>? categoryIds = null,
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
                  && (name == null || EF.Functions.ILike(product.Name, $"%{name}%") ||
                      EF.Functions.TrigramsWordSimilarity(product.Name, name) > SimilarityThreshold)
                  && (minRating == null || product.Rating >= minRating)
                  && (maxRating == null || product.Rating <= maxRating)
                  && (minPrice == null ||
                      price.Price - (promotion == null ? 0 : price.Price * (promotion.Percentage / 100)) >= minPrice)
                  && (maxPrice == null ||
                      price.Price - (promotion == null ? 0 : price.Price * (promotion.Percentage / 100)) <= maxPrice)
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
            .Where(p => (brandIds == null || brandIds.Contains(p.Product.BrandId)) &&
                        (companyIds == null || companyIds.Contains(p.Company.Id)) &&
                        (storeIds == null || storeIds.Contains(p.Store.Id)) &&
                        (cityIds == null || cityIds.Contains(p.City.Id)))
            .GroupBy(p => p.Product.CategoryId)
            .Select(grouping => new FacetCounter
            {
                Id = grouping.Key,
                Name = grouping.First().Category.Name,
                Count = grouping.GroupBy(g => g.Product.Id).Count()
            }).ToListAsync();

        var brandFacets = await query
            .Where(p => (categoryIds == null || categoryIds.Contains(p.Product.CategoryId)) &&
                        (companyIds == null || companyIds.Contains(p.Company.Id)) &&
                        (storeIds == null || storeIds.Contains(p.Store.Id)) &&
                        (cityIds == null || cityIds.Contains(p.City.Id)))
            .GroupBy(p => p.Product.BrandId)
            .Select(grouping => new FacetCounter
            {
                Id = grouping.Key,
                Name = grouping.First().Brand.Name,
                Count = grouping.GroupBy(g => g.Product.Id).Count()
            }).OrderByDescending(facet => facet.Count).Take(maxValuesPerFacet).ToListAsync();

        var companyFacets = await query
            .Where(p => (categoryIds == null || categoryIds.Contains(p.Product.CategoryId)) &&
                        (brandIds == null || brandIds.Contains(p.Product.BrandId)) &&
                        (storeIds == null || storeIds.Contains(p.Store.Id)) &&
                        (cityIds == null || cityIds.Contains(p.City.Id)))
            .GroupBy(p => p.Company.Id)
            .Select(grouping => new FacetCounter
            {
                Id = grouping.Key,
                Name = grouping.First().Company.Name,
                Count = grouping.GroupBy(g => g.Product.Id).Count()
            }).ToListAsync();

        var facetedQuery = query
            .Where(p =>
                (categoryIds == null || categoryIds.Contains(p.Product.CategoryId))
                && (brandIds == null || brandIds.Contains(p.Product.BrandId))
                && (companyIds == null || companyIds.Contains(p.Company.Id))
                && (storeIds == null || storeIds.Contains(p.Store.Id))
                && (cityIds == null || cityIds.Contains(p.City.Id))
            );

        var orderedQuery = sortBy switch
        {
            ProductsSortOption.Popularity => facetedQuery.OrderBy(queryRes => queryRes.Product.Views),
            ProductsSortOption.NameLowToHigh => facetedQuery.OrderBy(queryRes => queryRes.Product.Name),
            ProductsSortOption.NameHighToLow => facetedQuery.OrderByDescending(queryRes => queryRes.Product.Name),
            ProductsSortOption.RatingLowToHigh => facetedQuery.OrderBy(queryRes => queryRes.Product.Rating),
            ProductsSortOption.RatingHighToLow => facetedQuery.OrderByDescending(queryRes => queryRes.Product.Rating),
            ProductsSortOption.PriceLowToHigh => facetedQuery.OrderBy(queryRes =>
                queryRes.Price.Price - (queryRes.Promotion == null
                    ? 0
                    : queryRes.Price.Price * (queryRes.Promotion.Percentage / 100))),
            ProductsSortOption.PriceHighToLow => facetedQuery.OrderByDescending(queryRes =>
                queryRes.Price.Price - (queryRes.Promotion == null
                    ? 0
                    : queryRes.Price.Price * (queryRes.Promotion.Percentage / 100))),
            _ => facetedQuery
        };

        var bestOffers = orderedQuery
            .GroupBy(g => g.Product.Id)
            .Select(group =>
                group.Select(g => new ProductOffer(
                    g.Product.ToProduct(g.Brand.ToBrand(), g.Category.ToCategory()),
                    new StoreOffer(
                        g.Store.ToStore(
                            g.City == null ? null : g.City.ToCity(),
                            g.Company.ToCompany()
                        ),
                        new Price(g.Price.Price,
                            g.Promotion == null ? null : g.Promotion.ToPromotion(),
                            g.Price.CreatedAt),
                        g.Availability.ToStoreAvailability()
                    )
                ))
            )
            .Skip(skip)
            .Take(take)
            .ToList()
            .Select(group => group.MinBy(g => g.StoreOffer!.PriceData.FinalPrice)!);

        bestOffers = sortBy switch
        {
            ProductsSortOption.Popularity => bestOffers.OrderBy(queryRes => queryRes.Product.Views).ToList(),
            ProductsSortOption.NameLowToHigh => bestOffers.OrderBy(queryRes => queryRes.Product.Name).ToList(),
            ProductsSortOption.NameHighToLow =>
                bestOffers.OrderByDescending(queryRes => queryRes.Product.Name).ToList(),
            ProductsSortOption.RatingLowToHigh => bestOffers.OrderBy(queryRes => queryRes.Product.Rating).ToList(),
            ProductsSortOption.RatingHighToLow => bestOffers.OrderByDescending(queryRes => queryRes.Product.Rating)
                .ToList(),
            ProductsSortOption.PriceLowToHigh => bestOffers
                .OrderBy(queryRes => queryRes.StoreOffer!.PriceData.FinalPrice)
                .ToList(),
            ProductsSortOption.PriceHighToLow => bestOffers
                .OrderByDescending(queryRes => queryRes.StoreOffer!.PriceData.FinalPrice)
                .ToList(),
            _ => bestOffers
        };

        var productsFacetsCounters = new ProductsFacetsCounters(brandFacets, categoryFacets, companyFacets);

        var paginatedProducts = new PaginatedResult<ProductOffer>(bestOffers,
            query.GroupBy(g => g.Product.Id).Count(), skip, take);

        return new PaginatedProductOffers(paginatedProducts, productsFacetsCounters);
    }

    public async Task<StoreOffer?> GetCheapestStoreOfferAvailableByProductIdAsync(
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
                Promotion = promotion,
                Availability = availability
            };

        if (!query.Any())
        {
            return null;
        }

        return (await query.Select(g =>
                new StoreOffer(
                    g.Store.ToStore(g.City == null ? null : g.City.ToCity(), g.Company.ToCompany()),
                    new Price(
                        g.PriceEntry.Price,
                        g.Promotion == null ? null : g.Promotion.ToPromotion(),
                        g.PriceEntry.CreatedAt),
                    g.Availability.ToStoreAvailability()
                )
            ).ToListAsync())
            .MinBy(storeOffer => storeOffer.PriceData.FinalPrice);
    }

    public async Task<IEnumerable<StoreAvailability>> GetStoresAvailabilityAsync(string productId)
    {
        var query = dataContext.ProductAvailability.AsQueryable();

        return await query
            .Where(availability => availability.ProductId == productId)
            .Select(availabilityEntity => new StoreAvailability(
                availabilityEntity.StoreId,
                availabilityEntity.ProductId,
                availabilityEntity.IsAvailable,
                availabilityEntity.LastChecked
            ))
            .ToListAsync();
    }

    public async Task<StoreAvailability?> GetStoreAvailabilityStatusAsync(string productId, int storeId)
    {
        var queryRes = await (
            from availability in dataContext.ProductAvailability
            where availability.ProductId == productId && availability.StoreId == storeId
            select new { availability }
        ).FirstOrDefaultAsync();

        if (queryRes is null)
        {
            return null;
        }

        return new StoreAvailability(
            queryRes.availability.StoreId,
            queryRes.availability.ProductId,
            queryRes.availability.IsAvailable,
            queryRes.availability.LastChecked
        );
    }

    public async Task<StoreOffer?> GetStoreOfferAsync(
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
            join availability in dataContext.ProductAvailability on store.Id equals availability.StoreId
            where availability.ProductId == productId
            from promotion in promotionGroup.DefaultIfEmpty()
            select new
            {
                Store = store,
                City = city,
                Company = company,
                PriceEntry = priceEntry,
                Promotion = promotion,
                Availability = availability
            };

        return await query
            .Select(g =>
                new StoreOffer(
                    g.Store.ToStore(g.City == null ? null : g.City.ToCity(), g.Company.ToCompany()),
                    new Price(
                        g.PriceEntry.Price,
                        g.Promotion == null ? null : g.Promotion.ToPromotion(),
                        g.PriceEntry.CreatedAt),
                    g.Availability.ToStoreAvailability()
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

    public async Task<PriceId> AddPriceAsync(
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
            Price = price,
            CreatedAt = DateTime.Now
        };

        await dataContext.PriceEntry.AddAsync(priceEntry);

        await dataContext.SaveChangesAsync();

        if (promotionPercentage.HasValue)
        {
            var promotion = new PromotionEntity
            {
                Percentage = promotionPercentage.Value,
                PriceEntryId = priceEntry.Id,
                CreatedAt = DateTime.Now
            };

            await dataContext.Promotion.AddAsync(promotion);
        }

        await dataContext.SaveChangesAsync();

        return new PriceId(priceEntry.Id);
    }
}