using System.ComponentModel.DataAnnotations;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Http.Models.Product;

public class ProductPreferencesInputModel
{
    public Optional<bool> IsFavourite { get; set; }
    public Optional<PriceAlertInputModel?> PriceAlert { get; set; }
    public Optional<ProductReviewInputModel?> Review { get; set; }
}

public record PriceAlertInputModel([Required] int PriceThreshold);

public record ProductReviewInputModel(
    [Required] [Range(1, 5)] int Rating,
    [MaxLength(255)] string? Comment
);
