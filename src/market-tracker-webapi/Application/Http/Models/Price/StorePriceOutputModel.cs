using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.City;

namespace market_tracker_webapi.Application.Http.Models.Price;

public record StorePriceOutputModel(
    int Id,
    string Name,
    string Address,
    CityInfo? City,
    bool IsOnline,
    int Price,
    Promotion? Promotion,
    bool IsAvailable,
    DateTime LastChecked
)
{
    public static StorePriceOutputModel ToStorePriceOutputModel(
        Domain.Store store,
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
            city,
            city is null,
            price,
            promotion,
            isAvailable,
            lastChecked
        );
    }
}
