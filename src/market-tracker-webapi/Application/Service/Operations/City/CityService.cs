using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Repository.Operations.City;
using market_tracker_webapi.Application.Service.Errors.City;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.City;

public class CityService(ICityRepository cityRepository, ITransactionManager transactionManager)
    : ICityService
{
    public async Task<CollectionOutputModel> GetCitiesAsync()
    {
        var cities = await cityRepository.GetCitiesAsync();
        return new CollectionOutputModel(cities);
    }

    public async Task<Either<CityFetchingError, Domain.City>> GetCityByIdAsync(int id)
    {
        var city = await cityRepository.GetCityByIdAsync(id);
        return city is null
            ? EitherExtensions.Failure<CityFetchingError, Domain.City>(
                new CityFetchingError.CityByIdNotFound(id)
            )
            : EitherExtensions.Success<CityFetchingError, Domain.City>(city);
    }

    public async Task<Either<CityFetchingError, Domain.City>> GetCityByNameAsync(string cityName)
    {
        var city = await cityRepository.GetCityByNameAsync(cityName);
        return city is null
            ? EitherExtensions.Failure<CityFetchingError, Domain.City>(
                new CityFetchingError.CityByNameNotFound(cityName)
            )
            : EitherExtensions.Success<CityFetchingError, Domain.City>(city);
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
            return EitherExtensions.Success<ICityError, IdOutputModel>(new IdOutputModel(cityId));
        });
    }

    public async Task<Either<CityFetchingError, IdOutputModel>> DeleteCityAsync(int id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var city = await cityRepository.GetCityByIdAsync(id);
            if (city is null)
            {
                return EitherExtensions.Failure<CityFetchingError, IdOutputModel>(
                    new CityFetchingError.CityByIdNotFound(id)
                );
            }

            await cityRepository.DeleteCityAsync(id);
            return EitherExtensions.Success<CityFetchingError, IdOutputModel>(
                new IdOutputModel(id)
            );
        });
    }

    public async Task<Either<ICityError, Domain.City>> UpdateCityAsync(int id, string cityName)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await cityRepository.GetCityByNameAsync(cityName) is not null)
            {
                return EitherExtensions.Failure<ICityError, Domain.City>(
                    new CityCreationError.CityNameAlreadyExists(cityName)
                );
            }

            var city = await cityRepository.UpdateCityAsync(id, cityName);
            return city is null
                ? EitherExtensions.Failure<ICityError, Domain.City>(
                    new CityFetchingError.CityByIdNotFound(id)
                )
                : EitherExtensions.Success<ICityError, Domain.City>(city);
        });
    }
}
