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

        var list = await query.Select(g =>
            new StoreOffer(
                g.Store.ToStore(g.City == null ? null : g.City.ToCity(), g.Company.ToCompany()),
                new Price(
                    g.PriceEntry.Price,
                    g.Promotion == null ? null : g.Promotion.ToPromotion(),
                    g.PriceEntry.CreatedAt),
                g.Availability.ToStoreAvailability()
            )
        ).ToListAsync();

        return list
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
            CreatedAt = DateTime.UtcNow
        };

        await dataContext.PriceEntry.AddAsync(priceEntry);

        await dataContext.SaveChangesAsync();

        if (promotionPercentage.HasValue)
        {
            var promotion = new PromotionEntity
            {
                Percentage = promotionPercentage.Value,
                PriceEntryId = priceEntry.Id,
                CreatedAt = DateTime.UtcNow
            };

            await dataContext.Promotion.AddAsync(promotion);
        }

        await dataContext.SaveChangesAsync();

        return new PriceId(priceEntry.Id);
    }
}