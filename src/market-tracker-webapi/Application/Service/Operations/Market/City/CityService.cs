using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Repository.Market.City;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.City;
using market_tracker_webapi.Application.Service.Transaction;

namespace market_tracker_webapi.Application.Service.Operations.Market.City;

using City = Domain.Schemas.Market.Retail.Shop.City;

public class CityService(ICityRepository cityRepository, ITransactionManager transactionManager)
    : ICityService
{
    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
        return await transactionManager.ExecuteAsync(async () => await cityRepository.GetCitiesAsync());
    }

    public async Task<City> GetCityByIdAsync(int id)
    {
        return await cityRepository.GetCityByIdAsync(id) ?? throw new MarketTrackerServiceException(
            new CityFetchingError.CityByIdNotFound(id)
        );
    }

    public async Task<City> GetCityByNameAsync(string cityName)
    {
        return await cityRepository.GetCityByNameAsync(cityName) ?? throw new MarketTrackerServiceException(
            new CityFetchingError.CityByNameNotFound(cityName)
        );
    }

    public async Task<CityId> AddCityAsync(string cityName)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await cityRepository.GetCityByNameAsync(cityName) is not null)
            {
                throw new MarketTrackerServiceException(
                    new CityCreationError.CityNameAlreadyExists(cityName)
                );
            }

            return await cityRepository.AddCityAsync(cityName);
        });
    }

    public async Task<City> UpdateCityAsync(int id, string cityName)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await cityRepository.GetCityByNameAsync(cityName) is not null)
            {
                throw new MarketTrackerServiceException(
                    new CityCreationError.CityNameAlreadyExists(cityName)
                );
            }

            return await cityRepository.UpdateCityAsync(id, cityName) ?? throw new MarketTrackerServiceException(
                new CityFetchingError.CityByIdNotFound(id)
            );
        });
    }

    public async Task<CityId> DeleteCityAsync(int id)
    {
        return await transactionManager.ExecuteAsync(async () =>
            (await cityRepository.DeleteCityAsync(id))?.Id ??
            throw new MarketTrackerServiceException(new CityFetchingError.CityByIdNotFound(id))
        );
    }
}