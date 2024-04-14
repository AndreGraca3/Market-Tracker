using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Price;
using market_tracker_webapi.Application.Repository.Dto.Store;
using market_tracker_webapi.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.Prices;

public class PriceRepository(MarketTrackerDataContext dataContext) : IPriceRepository
{
    public async Task<StorePrice?> GetCheapestStorePriceByProductIdAsync(string productId, DateTime priceAt)
    {
        var query = await (
            from priceEntry in dataContext.PriceEntry
            where priceEntry.ProductId == productId
                  && priceEntry.CreatedAt <= priceAt
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
        ).ToListAsync();

        if (!query.Any())
        {
            return null;
        }

        var cheapestStore = query
            .Select(group =>
                new StorePrice(StoreInfo.ToStoreInfo(group.Store.ToStore(), group.City?.ToCity(),
                    group.Company.ToCompany()), PriceInfo.Calculate(group.PriceEntry.Price,
                    group.Promotion?.ToPromotion(group.PriceEntry.Price), group.PriceEntry.CreatedAt))
            ).OrderBy(group => group.PriceData.Price).First();

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

    public async Task<StorePrice> GetStorePriceByProductIdAsync(
        string productId,
        int storeId,
        DateTime priceAt
    )
    {
        var query = await (
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
        ).FirstAsync();

        return new StorePrice(
            StoreInfo.ToStoreInfo(
                query.Store.ToStore(),
                query.City?.ToCity(),
                query.Company.ToCompany()
            ),
            PriceInfo.Calculate(
                query.PriceEntry.Price,
                query.Promotion?.ToPromotion(query.PriceEntry.Price),
                query.PriceEntry.CreatedAt
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

    public Task AddPriceAsync(
        string productId,
        int storeId,
        int price,
        DateTime createdAt,
        int? promotionPercentage
    )
    {
        throw new NotImplementedException();
    }
}