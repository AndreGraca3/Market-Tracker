using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.City;

public class CityRepository(MarketTrackerDataContext marketTrackerDataContext) : ICityRepository
{
    public async Task<IEnumerable<Domain.City>> GetCitiesAsync()
    {
        var cityEntities = await marketTrackerDataContext.City.ToListAsync();
        return cityEntities.Select(MapCityEntity);
    }

    public async Task<Domain.City?> GetCityByIdAsync(int id)
    {
        var cityEntity = await marketTrackerDataContext.City.FindAsync(id);
        return cityEntity != null ? MapCityEntity(cityEntity) : null;
    }

    public async Task<Domain.City?> GetCityByNameAsync(string name)
    {
        var cityEntity = await marketTrackerDataContext.City.FirstOrDefaultAsync(c =>
            c.Name == name
        );
        return cityEntity != null ? MapCityEntity(cityEntity) : null;
    }

    public async Task<int> AddCityAsync(string name)
    {
        var newCity = new CityEntity { Name = name };

        marketTrackerDataContext.City.Add(newCity);
        await marketTrackerDataContext.SaveChangesAsync();

        return newCity.Id;
    }

    public async Task<Domain.City?> UpdateCityAsync(int id, string name)
    {
        var currentCity = await marketTrackerDataContext.City.FindAsync(id);

        if (currentCity == null)
        {
            return null;
        }

        currentCity.Name = name;

        await marketTrackerDataContext.SaveChangesAsync();
        return MapCityEntity(currentCity);
    }

    public async Task<Domain.City?> DeleteCityAsync(int id)
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

    private static Domain.City MapCityEntity(CityEntity cityEntity)
    {
        return new Domain.City { Id = cityEntity.Id, Name = cityEntity.Name };
    }
}
