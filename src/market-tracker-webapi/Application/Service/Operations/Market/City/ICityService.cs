using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.City;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.City;

using City = Domain.Models.Market.Retail.Shop.City;

public interface ICityService
{
    Task<Either<IServiceError, CollectionOutputModel<City>>> GetCitiesAsync();

    Task<Either<CityFetchingError, City>> GetCityByIdAsync(int id);

    Task<Either<CityFetchingError, City>> GetCityByNameAsync(string cityName);

    Task<Either<ICityError, IntIdOutputModel>> AddCityAsync(string cityName);

    Task<Either<CityFetchingError, IntIdOutputModel>> DeleteCityAsync(int id);

    Task<Either<ICityError, City>> UpdateCityAsync(int id, string cityName);
}
