using market_tracker_webapi.Application.Domain.Models.List;

namespace market_tracker_webapi.Application.Service.Results;

public class ShoppingListEntriesResult
{
    public required IEnumerable<ListEntryOffer> Products { get; set; }
    public int TotalPrice { get; set; }
    public int TotalProducts { get; set; }
}