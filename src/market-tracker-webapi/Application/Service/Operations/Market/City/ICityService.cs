using market_tracker_webapi.Application.Domain.Models.Market.Retail.Shop;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.City;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.City;

using City = Domain.Models.Market.Retail.Shop.City;

public interface ICityService
{
    Task<Either<IServiceError, IEnumerable<City>>> GetCitiesAsync();

    Task<Either<CityFetchingError, City>> GetCityByIdAsync(int id);

    Task<Either<CityFetchingError, City>> GetCityByNameAsync(string cityName);

    Task<Either<ICityError, CityId>> AddCityAsync(string cityName);

    Task<Either<ICityError, City>> UpdateCityAsync(int id, string cityName);

    Task<Either<CityFetchingError, CityId>> DeleteCityAsync(int id);
}