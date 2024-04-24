namespace market_tracker_webapi.Application.Http.Models.Token;

public record TokenOutputModel(Guid TokenValue, DateTime ExpiresAt);