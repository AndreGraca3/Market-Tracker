namespace market_tracker_webapi.Application.Repository.Dto.City;

public record CityInfo(int Id, string Name)
{
    public static CityInfo ToCity(Domain.City city)
    {
        return new CityInfo(city.Id, city.Name);
    }
}