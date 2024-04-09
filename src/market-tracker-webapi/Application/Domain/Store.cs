using market_tracker_webapi.Application.Repository.Dto.Store;

namespace market_tracker_webapi.Application.Domain;

public class Store
{
    public int Id { get; set; }

    public required string Name { get; set; }
    public required string Address { get; set; }

    public int? CityId { get; set; }

    public int CompanyId { get; set; }

    public bool IsOnline => CityId is null;

    public static Store ToStore(StoreInfo storeDetails)
    {
        return new Store
        {
            Id = storeDetails.Id,
            Name = storeDetails.Name,
            Address = storeDetails.Address,
            CityId = storeDetails.City?.Id,
            CompanyId = storeDetails.Company.Id
        };
    }
}