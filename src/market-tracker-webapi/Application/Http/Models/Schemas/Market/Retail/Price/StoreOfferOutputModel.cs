using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Price;

using Price = Domain.Schemas.Market.Retail.Sales.Pricing.Price;

public record StoreOfferOutputModel(
    StoreOutputModel Store,
    Price Price,
    bool IsAvailable,
    DateTime LastChecked
);

public record StoreOfferItemOutputModel(
    StoreDetailsOutputModel Store,
    Price Price,
    bool IsAvailable,
    DateTime LastChecked
);

public static class StoreOfferOutputModelMapper
{
    public static StoreOfferOutputModel ToOutputModel(this StoreOffer storeOffer)
    {
        return new StoreOfferOutputModel(
            storeOffer.Store.ToOutputModel(),
            storeOffer.PriceData,
            storeOffer.StoreAvailability.IsAvailable,
            storeOffer.StoreAvailability.LastChecked
        );
    }

    public static StoreOfferItemOutputModel ToStoreOfferItemOutputModel(this StoreOffer storeOfferItem)
    {
        return new StoreOfferItemOutputModel(
            storeOfferItem.Store.ToStoreDetailsOutputModel(),
            storeOfferItem.PriceData,
            storeOfferItem.StoreAvailability.IsAvailable,
            storeOfferItem.StoreAvailability.LastChecked
        );
    }
}