using market_tracker_webapi.Application.Domain.Models.Market.Retail.Shop;

namespace market_tracker_webapi.Application.Repository.Market.Store
{
    public interface IStoreRepository
    {
        Task<IEnumerable<Domain.Models.Market.Retail.Shop.Store>> GetStoresAsync(int? companyId = null, int? cityId = null, string? storeName = null);

        Task<Domain.Models.Market.Retail.Shop.Store?> GetStoreByIdAsync(int id);

        Task<Domain.Models.Market.Retail.Shop.Store?> GetStoreByNameAsync(string name);

        Task<Domain.Models.Market.Retail.Shop.Store?> GetStoreByOperatorIdAsync(Guid operatorId);

        Task<int> AddStoreAsync(string name, string address, int? cityId, int companyId);

        Task<StoreItem?> UpdateStoreAsync(int id, string address, int cityId, int companyId);

        Task<StoreItem?> DeleteStoreAsync(int id);
    }
}
