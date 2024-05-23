using System.ComponentModel.DataAnnotations;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product.Feedback;

public record ProductPreferencesInputModel(
    Optional<bool> IsFavourite,
    Optional<ProductReviewInputModel?> Review
);

public record ProductReviewInputModel(
    [Required] [Range(1, 5)] int Rating,
    [MaxLength(255)] string? Comment
);