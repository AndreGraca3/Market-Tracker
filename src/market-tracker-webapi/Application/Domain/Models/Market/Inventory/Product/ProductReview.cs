using market_tracker_webapi.Application.Domain.Models.Account.Users;

namespace market_tracker_webapi.Application.Domain.Models.Market.Inventory.Product;

public record ProductReview(
    int Id,
    ClientItem Client,
    string ProductId,
    int Rating,
    string? Text,
    DateTime CreatedAt
);