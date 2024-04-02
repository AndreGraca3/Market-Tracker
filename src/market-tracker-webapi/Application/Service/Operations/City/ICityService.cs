using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Service.Errors.City;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.City;

public interface ICityService
{
    Task<CollectionOutputModel> GetCitiesAsync();

    Task<Either<CityFetchingError, Domain.City>> GetCityByIdAsync(int id);

    Task<Either<CityFetchingError, Domain.City>> GetCityByNameAsync(string cityName);

    Task<Either<ICityError, IdOutputModel>> AddCityAsync(string cityName);

    Task<Either<CityFetchingError, IdOutputModel>> DeleteCityAsync(int id);

    Task<Either<ICityError, Domain.City>> UpdateCityAsync(int id, string cityName);
}
