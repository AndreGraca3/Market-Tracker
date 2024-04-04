namespace market_tracker_webapi.Application.Domain;

public record ProductReview(
    int Id,
    Guid ClientId,
    int ProductId,
    bool Moderated,
    int Rating,
    string? Text,
    DateTime CreatedAt
);
