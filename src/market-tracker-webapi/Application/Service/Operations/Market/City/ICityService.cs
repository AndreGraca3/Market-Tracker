using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;

namespace market_tracker_webapi.Application.Service.Operations.Market.City;

using City = Domain.Schemas.Market.Retail.Shop.City;

public interface ICityService
{
    Task<IEnumerable<City>> GetCitiesAsync();

    Task<City> GetCityByIdAsync(int id);

    Task<City> GetCityByNameAsync(string cityName);

    Task<CityId> AddCityAsync(string cityName);

    Task<City> UpdateCityAsync(int id, string cityName);

    Task<CityId> DeleteCityAsync(int id);
}