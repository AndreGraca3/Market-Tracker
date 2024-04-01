using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Repositories.City;
using market_tracker_webapi.Application.Service.Errors.City;
using market_tracker_webapi.Application.Services.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.City;

public class CityService(ICityRepository cityRepository, ITransactionManager transactionManager) : ICityService
{
    public Task<IEnumerable<CityDomain>> GetCitiesAsync()
    {
        var cities = cityRepository.GetCitiesAsync();
        return cities;
    }

    public async Task<Either<CityFetchingError, CityDomain>> GetCityByIdAsync(int id)
    {
        var city = await cityRepository.GetCityByIdAsync(id);
        return city is null
            ? EitherExtensions.Failure<CityFetchingError, CityDomain>(
                new CityFetchingError.CityByIdNotFound(id)
            )
            : EitherExtensions.Success<CityFetchingError, CityDomain>(city);
    }

    public async Task<Either<CityFetchingError, CityDomain>> GetCityByNameAsync(string cityName)
    {
        var city = await cityRepository.GetCityByNameAsync(cityName);
        return city is null
            ? EitherExtensions.Failure<CityFetchingError, CityDomain>(
                new CityFetchingError.CityByNameNotFound(cityName)
            )
            : EitherExtensions.Success<CityFetchingError, CityDomain>(city);
    }

    public async Task<Either<ICityError, IdOutputModel>> AddCityAsync(string cityName)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await cityRepository.GetCityByNameAsync(cityName) is not null)
            {
                return EitherExtensions.Failure<ICityError, IdOutputModel>(
                    new CityCreationError.CityNameAlreadyExists(cityName)
                );
            }
            
            var cityId = await cityRepository.AddCityAsync(cityName);
            return EitherExtensions.Success<ICityError, IdOutputModel>(
                new IdOutputModel
                {
                    Id = cityId
                }
            );
        });
    }

    public async Task<Either<CityFetchingError, IdOutputModel>> DeleteCityAsync(int id)
    {
        return await transactionManager.ExecuteAsync(async () => {
            var city = await cityRepository.GetCityByIdAsync(id);
            if (city is null)
            {
                return EitherExtensions.Failure<CityFetchingError, IdOutputModel>(
                    new CityFetchingError.CityByIdNotFound(id)
                );
            }

            await cityRepository.DeleteCityAsync(id);
            return EitherExtensions.Success<CityFetchingError, IdOutputModel>(
                new IdOutputModel
                {
                    Id = id
                }
            );
        });
    }

    public async Task<Either<ICityError, CityDomain>> UpdateCityAsync(int id, string cityName)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await cityRepository.GetCityByNameAsync(cityName) is not null)
            {
                return EitherExtensions.Failure<ICityError, CityDomain>(
                    new CityCreationError.CityNameAlreadyExists(cityName)
                );
            }
            
            var city = await cityRepository.UpdateCityAsync(id, cityName);
            return city is null
                ? EitherExtensions.Failure<ICityError, CityDomain>(
                    new CityFetchingError.CityByIdNotFound(id)
                )
                : EitherExtensions.Success<ICityError, CityDomain>(city);
        });
    }
}