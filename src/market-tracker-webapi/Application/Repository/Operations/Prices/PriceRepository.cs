using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace market_tracker_webapi.Application.Repository.Operations.Prices;

public class PriceRepository(MarketTrackerDataContext dataContext) : IPriceRepository
{
    public Task<StorePrice> GetCheapestStorePriceByProductIdAsync(
        string productId,
        DateTime priceAt
    )
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<StoreAvailability>> GetStoresAvailabilityByProductIdAsync(
        string productId,
        DateTime date
    )
    {
        return await dataContext
            .ProductAvailability.Where(availability => availability.ProductId == productId)
            .Join(
                dataContext.Company,
                availability => availability.StoreId,
                company => company.Id,
                (availability, company) => new { availability, company }
            )
            .Select(query => new StoreAvailability(
                query.availability.StoreId,
                query.availability.ProductId,
                query.company.Id,
                query.availability.IsAvailable,
                query.availability.LastChecked
            ))
            .ToListAsync();
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
            new PriceEntry(
                query.PriceEntry.Price,
                query.Promotion?.ToPromotion(),
                query.PriceEntry.CreatedAt
            )
        );
    }

    public Task<IEnumerable<PriceEntry>> GetPriceHistoryByProductIdAndStoreIdAsync(
        string productId,
        int storeId,
        DateTime pricedAfter,
        DateTime pricedBefore
    )
    {
        throw new NotImplementedException();
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
