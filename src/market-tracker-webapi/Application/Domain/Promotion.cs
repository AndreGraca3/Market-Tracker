namespace market_tracker_webapi.Application.Domain;

public record Promotion(int Percentage, DateTime CreatedAt, string PriceEntryId);
