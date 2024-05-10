namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Auth.Token;

public record TokenOutputModel(Guid TokenValue, DateTime ExpiresAt);