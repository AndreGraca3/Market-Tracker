using market_tracker_webapi.Application.Domain.Models.Market.Retail.Shop;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.Store;

using Store = Domain.Models.Market.Retail.Shop.Store;

public interface IStoreService
{
    Task<Either<IServiceError, CollectionOutputModel<Store>>> GetStoresAsync(
        int? companyId = null,
        int? cityId = null, string? name = null);

    Task<Either<StoreFetchingError, Store>> GetStoreByIdAsync(int id);

    Task<Either<IServiceError, StoreId>> AddStoreAsync(
        string name,
        string address,
        int? cityId,
        int companyId
    );

    Task<Either<IServiceError, StoreId>> UpdateStoreAsync(
        int id,
        string name,
        string address,
        int cityId,
        int companyId
    );

    Task<Either<StoreFetchingError, StoreId>> DeleteStoreAsync(int id);
}