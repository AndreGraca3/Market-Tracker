namespace market_tracker_webapi.Application.Service.Errors.City;

public class CityCreationError : ICityError
{
    public class CityNameAlreadyExists(string cityName) : CityCreationError
    {
        public string Name { get; } = cityName;
    }
}