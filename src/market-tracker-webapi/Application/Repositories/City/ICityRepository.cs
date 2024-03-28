using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repositories.City;

public interface ICityRepository
{
    Task<IEnumerable<CityDomain>> GetCitiesAsync();
    Task<CityDomain?> GetCityByIdAsync(int id);
    
    Task<CityDomain?> GetCityByNameAsync(string name);
    
    Task<int> AddCityAsync(string name);
    
    Task<CityDomain?> UpdateCityAsync(int id, string name);
    
    Task<CityDomain?> DeleteCityAsync(int id);
}