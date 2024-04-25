namespace market_tracker_webapi.Application.Http.Models.ListEntry;

public class ListAlternativeFiltersInputModel
{
    public IList<int>? CategoryIds { get; set; }

    public IList<int>? CompanyIds { get; set; }

    public IList<int>? StoreIds { get; set; }
    
    public IList<int>? CityIds { get; set; }
}