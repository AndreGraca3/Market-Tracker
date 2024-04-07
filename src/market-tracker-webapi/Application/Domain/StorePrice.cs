using market_tracker_webapi.Application.Http.Models.Store;

namespace market_tracker_webapi.Application.Domain;

public record StorePrice(StoreInfo Store, PriceEntry PriceDetails)
{
    public static StorePrice ToStorePrice(StoreInfo store, PriceEntry priceDetails)
    {
        return new StorePrice(store, priceDetails);
    }
}
