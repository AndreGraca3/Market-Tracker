using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Price;

using City = Domain.Schemas.Market.Retail.Shop.City;
using Price = Domain.Schemas.Market.Retail.Sales.Pricing.Price;

public record StoreOfferOutputModel(
    int Id,
    string Name,
    string Address,
    bool IsOnline,
    City? City,
    Price Price,
    bool IsAvailable
);

public static class StoreOfferOutputModelMapper
{
    public static StoreOfferOutputModel ToOutputModel(
        this StoreOffer storeOffer,
        bool isAvailable
    )
    {
        return new StoreOfferOutputModel(
            storeOffer.Store.Id.Value,
            storeOffer.Store.Name,
            storeOffer.Store.Address,
            storeOffer.Store.IsOnline,
            storeOffer.Store.City,
            storeOffer.PriceData,
            isAvailable
        );
    }
}
