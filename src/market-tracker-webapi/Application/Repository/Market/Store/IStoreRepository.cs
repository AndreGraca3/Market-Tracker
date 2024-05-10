using market_tracker_webapi.Application.Domain.Models.Market.Retail.Shop;

namespace market_tracker_webapi.Application.Repository.Market.Store;

using Store = Domain.Models.Market.Retail.Shop.Store;

public interface IStoreRepository
{
    Task<IEnumerable<Store>> GetStoresAsync(int? companyId = null, int? cityId = null, string? storeName = null);

    Task<Store?> GetStoreByIdAsync(int id);

    Task<Store?> GetStoreByNameAsync(string name);

    Task<Store?> GetStoreByOperatorIdAsync(Guid operatorId);

    Task<StoreId> AddStoreAsync(string name, string address, int? cityId, int companyId);

    Task<StoreItem?> UpdateStoreAsync(int id, string address, int cityId, int companyId);

    Task<StoreItem?> DeleteStoreAsync(int id);
}