using market_tracker_webapi.Application.Domain.Models.Account.Users;

namespace market_tracker_webapi.Application.Domain.Models.Market.Inventory.Product;

public record ProductReview(
    ReviewId Id,
    ClientItem Client,
    string ProductId,
    int Rating,
    string? Text,
    DateTime CreatedAt
)
{
    public ProductReview(
        int Id,
        ClientItem Client,
        string ProductId,
        int Rating,
        string? Text,
        DateTime CreatedAt
    ) : this(
        new ReviewId(Id),
        Client,
        ProductId,
        Rating,
        Text,
        CreatedAt
    )
    {
    }
};

public record ReviewId(
    int Value
);