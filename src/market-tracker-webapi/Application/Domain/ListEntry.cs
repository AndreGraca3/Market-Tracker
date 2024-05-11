namespace market_tracker_webapi.Application.Domain;

public class ListEntry
{
    public int ListId { get; set; }
    
    public required string ProductId { get; set; }
    
    public int? StoreId { get; set; }
    
    public int Quantity { get; set; }
}