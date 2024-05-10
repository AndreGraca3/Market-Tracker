﻿using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Repository.Market.City;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.City;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.City;

using City = Domain.Models.Market.Retail.Shop.City;

public class CityService(ICityRepository cityRepository, ITransactionManager transactionManager)
    : ICityService
{
    public async Task<Either<IServiceError, CollectionOutputModel<City>>> GetCitiesAsync()
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var cities = await cityRepository.GetCitiesAsync();
            return EitherExtensions.Success<IServiceError, CollectionOutputModel<City>>(
                new CollectionOutputModel<City>(cities)
            );
        });
    }

    public async Task<Either<CityFetchingError, City>> GetCityByIdAsync(int id)
    {
        var city = await cityRepository.GetCityByIdAsync(id);
        return city is null
            ? EitherExtensions.Failure<CityFetchingError, City>(
                new CityFetchingError.CityByIdNotFound(id)
            )
            : EitherExtensions.Success<CityFetchingError, City>(city);
    }

    public async Task<Either<CityFetchingError, City>> GetCityByNameAsync(string cityName)
    {
        var city = await cityRepository.GetCityByNameAsync(cityName);
        return city is null
            ? EitherExtensions.Failure<CityFetchingError, City>(
                new CityFetchingError.CityByNameNotFound(cityName)
            )
            : EitherExtensions.Success<CityFetchingError, City>(city);
    }

    public async Task<Either<ICityError, IntIdOutputModel>> AddCityAsync(string cityName)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await cityRepository.GetCityByNameAsync(cityName) is not null)
            {
                return EitherExtensions.Failure<ICityError, IntIdOutputModel>(
                    new CityCreationError.CityNameAlreadyExists(cityName)
                );
            }

            var cityId = await cityRepository.AddCityAsync(cityName);
            return EitherExtensions.Success<ICityError, IntIdOutputModel>(
                new IntIdOutputModel(cityId)
            );
        });
    }

    public async Task<Either<CityFetchingError, IntIdOutputModel>> DeleteCityAsync(int id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var city = await cityRepository.GetCityByIdAsync(id);
            if (city is null)
            {
                return EitherExtensions.Failure<CityFetchingError, IntIdOutputModel>(
                    new CityFetchingError.CityByIdNotFound(id)
                );
            }

            await cityRepository.DeleteCityAsync(id);
            return EitherExtensions.Success<CityFetchingError, IntIdOutputModel>(
                new IntIdOutputModel(id)
            );
        });
    }

    public async Task<Either<ICityError, City>> UpdateCityAsync(int id, string cityName)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await cityRepository.GetCityByNameAsync(cityName) is not null)
            {
                return EitherExtensions.Failure<ICityError, City>(
                    new CityCreationError.CityNameAlreadyExists(cityName)
                );
            }

            var city = await cityRepository.UpdateCityAsync(id, cityName);
            return city is null
                ? EitherExtensions.Failure<ICityError, City>(
                    new CityFetchingError.CityByIdNotFound(id)
                )
                : EitherExtensions.Success<ICityError, City>(city);
        });
    }
}
