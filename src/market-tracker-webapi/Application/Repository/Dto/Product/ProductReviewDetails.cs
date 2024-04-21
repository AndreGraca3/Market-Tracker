namespace market_tracker_webapi.Application.Repository.Dto.Product;

public record ProductReviewDetails(
    string ProductId,
    string UserId,
    string UserName,
    int Rating,
    string? Text,
    DateTime CreatedAt
);
