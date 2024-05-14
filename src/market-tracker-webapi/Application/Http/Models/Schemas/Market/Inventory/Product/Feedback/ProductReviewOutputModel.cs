using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Client;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product.Feedback;

public record ProductReviewOutputModel(
    int Id,
    string ProductId,
    int Rating,
    string? Text,
    DateTime CreatedAt,
    ClientItemOutputModel Client
);

public static class ProductReviewOutputModelMapper
{
    public static ProductReviewOutputModel ToOutputModel(this ProductReview productReview)
    {
        return new ProductReviewOutputModel(
            productReview.Id.Value,
            productReview.ProductId,
            productReview.Rating,
            productReview.Text,
            productReview.CreatedAt,
            productReview.Client.ToOutputModel()
        );
    }
}