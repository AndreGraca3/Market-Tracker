namespace market_tracker_webapi.Application.Domain.Models.List;

public record ShoppingList(int Id, string Name, DateTime? ArchivedAt, DateTime CreatedAt, Guid OwnerId);