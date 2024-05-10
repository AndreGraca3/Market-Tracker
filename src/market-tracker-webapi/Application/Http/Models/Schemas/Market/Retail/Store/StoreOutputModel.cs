namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;

using Store = Domain.Models.Market.Retail.Shop.Store;

public class StoreOutputModel
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required string Address { get; set; }

    public int? CityId { get; set; }

    public required int CompanyId { get; set; }

    public bool IsOnline => CityId is null;

    public static StoreOutputModel ToStoreOutputModel(Store storeDetails)
    {
        return new StoreOutputModel
        {
            Id = storeDetails.Id.Value,
            Name = storeDetails.Name,
            Address = storeDetails.Address,
            CityId = storeDetails.City?.Id.Value,
            CompanyId = storeDetails.Company.Id.Value
        };
    }
}