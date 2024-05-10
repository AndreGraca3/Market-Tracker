namespace market_tracker_webapi.Application.Http.Models.Schemas.List.ListEntry;

public class AlternativeListFiltersInputModel
{
    public IList<int>? CategoryIds { get; set; }

    public IList<int>? CompanyIds { get; set; }

    public IList<int>? StoreIds { get; set; }
    
    public IList<int>? CityIds { get; set; }
}