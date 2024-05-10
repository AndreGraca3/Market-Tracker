using market_tracker_webapi.Application.Domain.Models.Market.Retail.Shop;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Market.City;

using City = Domain.Models.Market.Retail.Shop.City;

public class CityRepository(MarketTrackerDataContext marketTrackerDataContext) : ICityRepository
{
    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
        var cityEntities = await marketTrackerDataContext.City.ToListAsync();
        return cityEntities.Select(cityEntity => cityEntity.ToCity());
    }

    public async Task<City?> GetCityByIdAsync(int id)
    {
        var cityEntity = await marketTrackerDataContext.City.FindAsync(id);
        return cityEntity?.ToCity();
    }

    public async Task<City?> GetCityByNameAsync(string name)
    {
        var cityEntity = await marketTrackerDataContext.City.FirstOrDefaultAsync(c =>
            c.Name == name
        );
        return cityEntity?.ToCity();
    }

    public async Task<CityId> AddCityAsync(string name)
    {
        var newCity = new CityEntity { Name = name };

        marketTrackerDataContext.City.Add(newCity);
        await marketTrackerDataContext.SaveChangesAsync();

        return new CityId(newCity.Id);
    }

    public async Task<City?> UpdateCityAsync(int id, string name)
    {
        var currentCity = await marketTrackerDataContext.City.FindAsync(id);

        if (currentCity == null)
        {
            return null;
        }

        currentCity.Name = name;

        await marketTrackerDataContext.SaveChangesAsync();
        return currentCity.ToCity();
    }

    public async Task<City?> DeleteCityAsync(int id)
    {
        var cityEntity = await marketTrackerDataContext.City.FindAsync(id);

        if (cityEntity == null)
        {
            return null;
        }

        marketTrackerDataContext.City.Remove(cityEntity);
        await marketTrackerDataContext.SaveChangesAsync();

        return cityEntity.ToCity();
    }
}