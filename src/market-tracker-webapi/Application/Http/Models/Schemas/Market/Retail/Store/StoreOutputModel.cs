using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;

using Store = Domain.Schemas.Market.Retail.Shop.Store;

public record StoreOutputModel(int Id, string Name, string Address, int? CityId, int CompanyId)
{
    public bool IsOnline => CityId is null;
}

public record StoreItemOutputModel(int Id, string Name, string Address, int? CityId, int CompanyId)
{
    public bool IsOnline => CityId is null;
}

public static class StoreOutputModelMapper
{
    public static StoreOutputModel ToOutputModel(this Store store)
    {
        return new StoreOutputModel(store.Id.Value, store.Name, store.Address, store.City?.Id.Value,
            store.Company.Id.Value);
    }

    public static StoreItemOutputModel ToOutputModel(this StoreItem storeItem)
    {
        return new StoreItemOutputModel(storeItem.Id.Value, storeItem.Name, storeItem.Address, storeItem.CityId,
            storeItem.CompanyId);
    }
}