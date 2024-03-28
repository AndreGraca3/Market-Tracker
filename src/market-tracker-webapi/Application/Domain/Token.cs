namespace market_tracker_webapi.Application.Domain;

public record Token(string TokenValue, DateTime CreatedAt, DateTime ExpiresAt, Guid UserId);