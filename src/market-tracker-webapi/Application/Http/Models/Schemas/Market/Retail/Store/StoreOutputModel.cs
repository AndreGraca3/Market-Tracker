using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.City;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Company;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;

using Store = Domain.Schemas.Market.Retail.Shop.Store;

public record StoreOutputModel(int Id, string Name, string Address, CityOutputModel? City, CompanyOutputModel Company)
{
    public bool IsOnline => City is null;
}

public record StoreInfoOutputModel(int Id, string Name, string Address, CityOutputModel? City)
{
    public bool IsOnline => City is null;
}

public record StoreItemOutputModel(int Id, string Name, string Address, int? CityId)
{
    public bool IsOnline => CityId is null;
}

public static class StoreOutputModelMapper
{
    public static StoreOutputModel ToOutputModel(this Store store)
    {
        return new StoreOutputModel(store.Id.Value, store.Name, store.Address, store.City?.ToOutputModel(),
            store.Company.ToOutputModel());
    }

    public static StoreItemOutputModel ToOutputModel(this StoreItem storeItem)
    {
        return new StoreItemOutputModel(storeItem.Id.Value, storeItem.Name, storeItem.Address, storeItem.CityId);
    }
    
    public static StoreInfoOutputModel ToStoreInfoOutputModel(this Store store)
    {
        return new StoreInfoOutputModel(store.Id.Value, store.Name, store.Address, store.City?.ToOutputModel());
    }
}