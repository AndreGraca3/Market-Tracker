namespace market_tracker_webapi.Application.Domain;

public class ShoppingList
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
    
    public DateTime? ArchivedAt { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public required Guid OwnerId { get; set; }
}