namespace market_tracker_webapi.Application.Http.Models.ListEntry;

public class ListEntryCreationInputModel
{
    public required string ProductId { get; set; }
    public required int StoreId { get; set; }
    public required int Quantity { get; set; }
}