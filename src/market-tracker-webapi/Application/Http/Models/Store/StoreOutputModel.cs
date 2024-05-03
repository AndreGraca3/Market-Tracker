using market_tracker_webapi.Application.Repository.Dto.Store;

namespace market_tracker_webapi.Application.Http.Models.Store;

public class StoreOutputModel
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required string Address { get; set; }

    public int? CityId { get; set; }

    public required int CompanyId { get; set; }

    public bool IsOnline => CityId is null;

    public static StoreOutputModel ToStoreOutputModel(StoreInfo storeDetails)
    {
        return new StoreOutputModel
        {
            Id = storeDetails.Id,
            Name = storeDetails.Name,
            Address = storeDetails.Address,
            CityId = storeDetails.City?.Id,
            CompanyId = storeDetails.Company.Id,
        };
    }
}