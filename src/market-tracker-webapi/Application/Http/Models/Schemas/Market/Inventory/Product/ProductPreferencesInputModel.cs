using System.ComponentModel.DataAnnotations;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product;

public class ProductPreferencesInputModel
{
    public Optional<bool> IsFavourite { get; set; }
    public Optional<ProductReviewInputModel?> Review { get; set; }
}

public record ProductReviewInputModel(
    [Required] [Range(1, 5)] int Rating,
    [MaxLength(255)] string? Comment
);
