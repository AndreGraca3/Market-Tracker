using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Domain.Models.Market.Price;
using market_tracker_webapi.Application.Http.Models.Store;
using market_tracker_webapi.Application.Repository.Dto.City;

namespace market_tracker_webapi.Application.Http.Models.Price;

public record StorePriceOutputModel(
    int Id,
    string Name,
    string Address,
    bool IsOnline,
    CityInfo? City,
    int Price,
    Promotion? Promotion,
    bool IsAvailable,
    DateTime LastChecked
)
{
    public static StorePriceOutputModel ToStorePriceOutputModel(
        StoreOutputModel store,
        CityInfo? city,
        int price,
        Promotion? promotion,
        bool isAvailable,
        DateTime lastChecked
    )
    {
        return new StorePriceOutputModel(
            store.Id,
            store.Name,
            store.Address,
            store.IsOnline,
            city,
            price,
            promotion,
            isAvailable,
            lastChecked
        );
    }
}
