using System.ComponentModel.DataAnnotations;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product;

public record ProductCreationInputModel(
    [Required] string Id,
    [Required]
    [MaxLength(100, ErrorMessage = "Name too long.")]
    string Name,
    [Required] string ImageUrl,
    [Required] string BrandName,
    [Required] int? CategoryId,
    [Required] int? Price,
    [Range(1, 100, ErrorMessage = "Promotion percentage must be between 1 and 100.")]
    int? PromotionPercentage,
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    int Quantity = 1,
    [Required] ProductUnit Unit = ProductUnit.Units
);