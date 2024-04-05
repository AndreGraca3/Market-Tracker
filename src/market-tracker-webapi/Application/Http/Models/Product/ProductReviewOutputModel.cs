using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models.User;

namespace market_tracker_webapi.Application.Http.Models.Product;

public record ProductReviewOutputModel(UserInfoOutputModel user, ProductReview review)
{
    public static ProductReviewOutputModel ToProductReviewOutputModel(
        UserInfoOutputModel user,
        ProductReview review
    ) => new ProductReviewOutputModel(user, review);
}
