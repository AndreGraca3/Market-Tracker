using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;

namespace market_tracker_webapi.Application.Service.Operations.Market.Store;

using Store = Domain.Schemas.Market.Retail.Shop.Store;

public interface IStoreService
{
    Task<IEnumerable<Store>> GetStoresAsync(
        int? companyId = null,
        int? cityId = null, string? name = null);

    Task<Store> GetStoreByIdAsync(int id);

    Task<StoreId> AddStoreAsync(
        string name,
        string address,
        int? cityId,
        int companyId,
        Guid operatorId
    );

    Task<StoreItem> UpdateStoreAsync(
        int id,
        string name,
        string address,
        int cityId,
        int companyId
    );

    Task<StoreId> DeleteStoreAsync(int id);
}