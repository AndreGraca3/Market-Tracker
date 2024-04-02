namespace market_tracker_webapi.Application.Http.Models.Store;

public class StoreCreationInputModel
{
    public required string Name { get; set; }

    public required string Address { get; set; }

    public required int CityId { get; set; }

    public required int CompanyId { get; set; }
}
