using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Product;

public record ReviewInputModel(
    [Required] [Range(1, 5)] int Rating,
    [MaxLength(255)] string? Comment
);
