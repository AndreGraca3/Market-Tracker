namespace market_tracker_webapi.Application.Http.Models.ListEntry;

public class ShoppingListEntriesOutputModel
{
    public required IEnumerable<ListEntryDetails> Products { get; set; }
    public int TotalPrice { get; set; }
    public int TotalProducts { get; set; }
}