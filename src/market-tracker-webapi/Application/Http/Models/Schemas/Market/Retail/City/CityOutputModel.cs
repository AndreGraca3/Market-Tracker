namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.City;

using City = Domain.Schemas.Market.Retail.Shop.City;

public record CityOutputModel(
    int Id,
    string Name
);

public static class CityOutputModelMapper
{
    public static CityOutputModel ToOutputModel(this City city)
    {
        return new CityOutputModel(city.Id.Value, city.Name);
    }
}