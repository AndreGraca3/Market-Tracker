namespace market_tracker_webapi.Application.Service.Errors.City;

public class CityFetchingError: ICityError
{
    public class CityByIdNotFound(int id) : CityFetchingError
    {
        public int Id { get; } = id;
    }
    
    public class CityByNameNotFound(string cityName) : CityFetchingError
    {
        public string Name { get; } = cityName;
    }
}