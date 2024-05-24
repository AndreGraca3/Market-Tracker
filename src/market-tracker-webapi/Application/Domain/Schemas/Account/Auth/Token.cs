namespace market_tracker_webapi.Application.Domain.Schemas.Account.Auth;

public record Token(Guid Value, DateTime CreatedAt, DateTime ExpiresAt, Guid UserId);
