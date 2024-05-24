using market_tracker_webapi.Application.Domain.Schemas.Account.Users;

namespace market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;

public record ProductReview(
    ReviewId Id,
    ClientItem Client,
    string ProductId,
    int Rating,
    string? Comment,
    DateTime CreatedAt
)
{
    public ProductReview(
        int Id,
        ClientItem Client,
        string ProductId,
        int Rating,
        string? Comment,
        DateTime CreatedAt
    ) : this(
        new ReviewId(Id),
        Client,
        ProductId,
        Rating,
        Comment,
        CreatedAt
    )
    {
    }
};

public record ReviewId(
    int Value
);