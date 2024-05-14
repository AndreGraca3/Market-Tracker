using market_tracker_webapi.Application.Domain.Schemas.List;

namespace market_tracker_webapi.Application.Service.Results;

public class ShoppingListEntriesResult
{
    public required IEnumerable<ListEntryOffer> Entries { get; set; }
    public int TotalPrice { get; set; }
    public int TotalProducts { get; set; }
}