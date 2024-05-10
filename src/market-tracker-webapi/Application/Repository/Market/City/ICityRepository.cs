namespace market_tracker_webapi.Application.Repository.Market.City;

using City = Domain.Models.Market.Retail.Shop.City;

public interface ICityRepository
{
    Task<IEnumerable<City>> GetCitiesAsync();
    Task<City?> GetCityByIdAsync(int id);

    Task<City?> GetCityByNameAsync(string name);

    Task<int> AddCityAsync(string name);

    Task<City?> UpdateCityAsync(int id, string name);

    Task<City?> DeleteCityAsync(int id);
}
