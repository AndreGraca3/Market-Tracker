using market_tracker_webapi.Application.Domain.Models.Account.Users;

namespace market_tracker_webapi.Application.Service.Results;

public class ShoppingListResult
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime? ArchivedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public required IEnumerable<UserId> ClientIds { get; set; }
}