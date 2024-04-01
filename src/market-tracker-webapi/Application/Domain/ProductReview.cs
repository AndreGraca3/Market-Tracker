namespace market_tracker_webapi.Application.Domain;

public record ProductReview(
    Guid ClientId,
    int ProductId,
    double Rate,
    string Comment,
    DateTime CreatedAt
);
