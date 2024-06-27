namespace market_tracker_webapi.Application.Http.Models.Schemas.List.ListEntry;

public record AlternativeListFiltersInputModel(
    IList<int>? CompanyIds,
    IList<int>? StoreIds,
    IList<int>? CityIds
);