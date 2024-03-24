using market_tracker_webapi.Application.Models.City;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using MarketTracker.Application.Repositories.City;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repositories.City;

public class CityRepository(MarketTrackerDataContext marketTrackerDataContext) : ICityRepository
{
    public async Task<CityData?> GetCityByIdAsync(int id)
    {
        var cityEntity = await marketTrackerDataContext.City.FindAsync(id);
        return cityEntity != null ? MapCityEntity(cityEntity) : null;
    }

    public async Task<CityData?> GetCityByNameAsync(string name)
    {
        var cityEntity = await marketTrackerDataContext.City.FirstOrDefaultAsync(c => c.Name == name);
        return cityEntity != null ? MapCityEntity(cityEntity) : null;
    }

    public async Task<int?> AddCityAsync(CityAddInputData cityData)
    {
        var newCity = new CityEntity
        {
            Name = cityData.Name
        };
        
        marketTrackerDataContext.City.Add(newCity);
        await marketTrackerDataContext.SaveChangesAsync();
        
        return newCity.Id;
    }

    public async Task<CityData?> UpdateCityAsync(CityUpdateInputData cityData)
    {
        var currentCity = await marketTrackerDataContext.City.FindAsync(cityData.Id);
        
        if (currentCity == null)
        {
            return null;
        }
        
        currentCity.Name = cityData.Name;
        
        await marketTrackerDataContext.SaveChangesAsync();
        return MapCityEntity(currentCity);
    }

    public async Task<CityData?> DeleteCityAsync(int id)
    {
        var cityEntity = await marketTrackerDataContext.City.FindAsync(id);
        
        if (cityEntity == null)
        {
            return null;
        }
        
        marketTrackerDataContext.City.Remove(cityEntity);
        await marketTrackerDataContext.SaveChangesAsync();
        
        return MapCityEntity(cityEntity);
    }
    
    private static CityData MapCityEntity(CityEntity cityEntity)
    {
        return new CityData
        {
            Id = cityEntity.Id,
            Name = cityEntity.Name
        };
    }
}