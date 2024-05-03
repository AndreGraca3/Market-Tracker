using market_tracker_webapi.Application.Repository.Dto.Store;

namespace market_tracker_webapi.Application.Domain;

public class Store
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required string Address { get; set; }

    public int? CityId { get; set; }

    public required int CompanyId { get; set; }
    
    public required Guid OperatorId { get; set; }

    public bool IsOnline => CityId is null;
}