using market_tracker_webapi.Application.Domain.Models.Market.Inventory.Product;

namespace market_tracker_webapi.Application.Http.Models.Product;

public record ProductReviewOutputModel(Domain.Models.Account.Users.User User, ProductReview Review)
{
    public static ProductReviewOutputModel ToProductReviewOutputModel(
        Domain.Models.Account.Users.User user,
        ProductReview review
    ) => new ProductReviewOutputModel(user, review);
}
