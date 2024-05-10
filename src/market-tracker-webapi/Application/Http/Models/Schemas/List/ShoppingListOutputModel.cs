namespace market_tracker_webapi.Application.Http.Models.Schemas.List;

public class ShoppingListOutputModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime? ArchivedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public required IEnumerable<Guid> ClientIds { get; set; }
}