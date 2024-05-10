namespace market_tracker_webapi.Application.Domain.Models.Market.Retail.Shop;

public record City(CityId Id, string Name)
{
    public City(
        int Id,
        string Name
    ) : this(
        new CityId(Id),
        Name
    )
    {
    }
};

public record CityId(
    int Value
);