namespace market_tracker_webapi.Application.Domain;

public record ProductReview(
    int Id,
    Guid ClientId,
    string ProductId,
    int Rating,
    string? Text,
    DateTime CreatedAt
);