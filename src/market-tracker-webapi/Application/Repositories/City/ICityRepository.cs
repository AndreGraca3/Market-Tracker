using market_tracker_webapi.Application.Models.City;

namespace MarketTracker.Application.Repositories.City;

public interface ICityRepository
{
    Task<CityData?> GetCityByIdAsync(int id);
    
    Task<CityData?> GetCityByNameAsync(string name);
    
    Task<int?> AddCityAsync(CityAddInputData cityData);
    
    Task<CityData?> UpdateCityAsync(CityUpdateInputData cityData);
    
    Task<CityData?> DeleteCityAsync(int id);
}