namespace market_tracker_webapi.Application.Domain;

public class ListOfProducts
{
    public int Id { get; set; }
    
    public Guid ClientId { get; set; }
    
    public required string ListName { get; set; }
    
    public DateTime? ArchivedAt { get; set; }
}