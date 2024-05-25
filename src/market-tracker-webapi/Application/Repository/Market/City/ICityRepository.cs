using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;

namespace market_tracker_webapi.Application.Repository.Market.City;

using City = Domain.Schemas.Market.Retail.Shop.City;

public interface ICityRepository
{
    Task<IEnumerable<City>> GetCitiesAsync();
    Task<City?> GetCityByIdAsync(int id);

    Task<City?> GetCityByNameAsync(string name);

    Task<CityId> AddCityAsync(string name);

    Task<City?> UpdateCityAsync(int id, string name);

    Task<City?> DeleteCityAsync(int id);
}