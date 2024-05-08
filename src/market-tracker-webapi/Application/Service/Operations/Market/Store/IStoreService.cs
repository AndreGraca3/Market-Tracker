using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.Store
{
    public interface IStoreService
    {
        Task<Either<IServiceError, CollectionOutputModel<Domain.Models.Market.Retail.Shop.Store>>> GetStoresAsync(int? companyId = null,
            int? cityId = null, string? name = null);

        Task<Either<StoreFetchingError, Domain.Models.Market.Retail.Shop.Store>> GetStoreByIdAsync(int id);

        Task<Either<IServiceError, IntIdOutputModel>> AddStoreAsync(
            string name,
            string address,
            int? cityId,
            int companyId
        );

        Task<Either<IServiceError, IntIdOutputModel>> UpdateStoreAsync(
            int id,
            string name,
            string address,
            int cityId,
            int companyId
        );

        Task<Either<StoreFetchingError, IntIdOutputModel>> DeleteStoreAsync(int id);
    }
}