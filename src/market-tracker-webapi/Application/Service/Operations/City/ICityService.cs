using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Service.Errors.City;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.City;

public interface ICityService
{
    Task<IEnumerable<CityDomain>> GetCitiesAsync();
    
    Task<Either<CityFetchingError, CityDomain>> GetCityByIdAsync(int id);
    
    Task<Either<CityFetchingError, CityDomain>> GetCityByNameAsync(string cityName);
    
    Task<Either<ICityError, IdOutputModel>> AddCityAsync(string cityName);
    
    Task<Either<CityFetchingError, IdOutputModel>> DeleteCityAsync(int id);
    
    Task<Either<ICityError, CityDomain>> UpdateCityAsync(int id, string cityName);
}