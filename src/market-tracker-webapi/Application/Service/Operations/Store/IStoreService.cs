using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Store
{
    public interface IStoreService
    {
        Task<CollectionOutputModel> GetStoresAsync();
        Task<Either<StoreFetchingError, Domain.Store>> GetStoreByIdAsync(int id);

        Task<Either<StoreFetchingError, IEnumerable<Domain.Store>>> GetStoresFromCompanyAsync(
            int companyId
        );

        Task<Either<StoreFetchingError, IEnumerable<Domain.Store>>> GetStoresByCityNameAsync(
            string cityName
        );

        Task<Either<IStoreError, IdOutputModel>> AddStoreAsync(
            string name,
            string address,
            int cityId,
            int companyId
        );

        Task<Either<IStoreError, IdOutputModel>> UpdateStoreAsync(
            int id,
            string name,
            string address,
            int cityId,
            int companyId
        );

        Task<Either<StoreFetchingError, IdOutputModel>> DeleteStoreAsync(int id);
    }
}
