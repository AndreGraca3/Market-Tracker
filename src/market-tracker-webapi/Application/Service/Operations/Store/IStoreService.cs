using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Store
{
    public interface IStoreService
    {
        Task<IEnumerable<StoreDomain>> GetStoresAsync();
        Task<Either<StoreFetchingError, StoreDomain>> GetStoreByIdAsync(int id);
        
        Task<Either<StoreFetchingError, IEnumerable<StoreDomain>>> GetStoresFromCompany(int companyId);
    
        Task<Either<StoreFetchingError, IEnumerable<StoreDomain>>> GetStoresFromCityByName(string cityName);
    
        Task<Either<IStoreError, IdOutputModel>> AddStoreAsync(string name, string address, int cityId, int companyId);
    
        Task<Either<StoreFetchingError, IdOutputModel>> UpdateStoreAsync(int id, string address, int cityId, int companyId);
        
        Task<Either<StoreFetchingError, IdOutputModel>> DeleteStoreAsync(int id);
    }
}