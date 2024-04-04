namespace market_tracker_webapi.Application.Domain;

public record PriceAlert(Guid ClientId, int ProductId, int PriceThreshold, DateTime CreatedAt);
