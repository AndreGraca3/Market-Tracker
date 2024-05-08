namespace market_tracker_webapi.Application.Domain.Models.Account.Auth;

public record Token(Guid TokenValue, DateTime CreatedAt, DateTime ExpiresAt, Guid UserId);
