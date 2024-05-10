using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Price;

using City = Domain.Models.Market.Retail.Shop.City;
using Price = Domain.Models.Market.Retail.Sales.Pricing.Price;

public record StoreOfferOutputModel(
    int Id,
    string Name,
    string Address,
    bool IsOnline,
    City? City,
    Price Price,
    bool IsAvailable,
    DateTime LastChecked
)
{
    public static StoreOfferOutputModel ToStoreOfferOutputModel(
        StoreOutputModel store,
        City? city,
        Price price,
        bool isAvailable,
        DateTime lastChecked
    )
    {
        return new StoreOfferOutputModel(
            store.Id,
            store.Name,
            store.Address,
            store.IsOnline,
            city,
            price,
            isAvailable,
            lastChecked
        );
    }
}
