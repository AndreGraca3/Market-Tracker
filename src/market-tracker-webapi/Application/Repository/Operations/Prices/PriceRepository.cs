using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.Prices;

public class PriceRepository(MarketTrackerDataContext dataContext) : IPriceRepository
{
    public Task<StorePrice> GetCheapestStorePriceByProductIdAsync(int productId, DateTime priceAt)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<StorePrice>> GetStoresAvailabilityByProductIdAsync(
        int productId,
        DateTime date
    )
    {
        throw new NotImplementedException();
    }

    public async Task<StorePrice> GetStorePriceByProductIdAsync(
        int productId,
        int storeId,
        DateTime priceAt
    )
    {
        var query = await (
            from price in dataContext.PriceEntry
            join store in dataContext.Store on price.StoreId equals store.Id
            join city in dataContext.City on store.CityId equals city.Id
            join company in dataContext.Company on store.CompanyId equals company.Id
            join promotion in dataContext.Promotion on price.PromotionId equals promotion.Id
            where
                price.ProductId == productId
                && price.StoreId == storeId
                && price.CreatedAt <= priceAt
            select new
            {
                Store = store,
                City = city,
                Company = company,
                Price = price.Price,
                Promotion = promotion,
            }
        ).FirstAsync();

        return new StorePrice(
            StoreInfo.ToStoreInfo(
                query.Store.ToStore(),
                query.City.ToCity(),
                query.Company.ToCompany()
            ),
            new PriceEntry(query.Price, query.Promotion.ToPromotion(), DateTime.Now)
        );
    }

    public Task<IEnumerable<PriceEntry>> GetStorePricesHistoryByProductIdAndStoreIdAsync(
        int productId,
        int storeId,
        DateTime pricedAfter,
        DateTime pricedBefore
    )
    {
        throw new NotImplementedException();
    }

    public Task AddPriceAsync(
        int productId,
        int storeId,
        int price,
        DateTime createdAt,
        int? promotionPercentage
    )
    {
        throw new NotImplementedException();
    }
}
