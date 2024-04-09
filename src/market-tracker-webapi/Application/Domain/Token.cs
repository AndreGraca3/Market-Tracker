namespace market_tracker_webapi.Application.Domain;

public record Token(Guid TokenValue, DateTime CreatedAt, DateTime ExpiresAt, Guid UserId);
