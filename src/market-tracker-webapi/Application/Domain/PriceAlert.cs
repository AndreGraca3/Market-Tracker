namespace market_tracker_webapi.Application.Domain;

public record PriceAlert(string Id, Guid ClientId, string ProductId, int PriceThreshold, DateTime CreatedAt);
