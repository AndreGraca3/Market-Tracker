namespace market_tracker_webapi.Application.Domain;

public class Store
{
    public int Id { get; set; }

    public required string Name { get; set; }
    public required string Address { get; set; }

    public int? CityId { get; set; }

    public int CompanyId { get; set; }

    public static Store ToStore(StoreInfo storeInfo)
    {
        return new Store
        {
            Id = storeInfo.Id,
            Name = storeInfo.Name,
            Address = storeInfo.Address,
            CityId = storeInfo.City?.Id,
            CompanyId = storeInfo.Company.Id
        };
    }
}
