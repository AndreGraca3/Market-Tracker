namespace market_tracker_webapi.Application.Domain;

public record PriceEntry(int Price, Promotion? Promotion, DateTime CreatedAt);
