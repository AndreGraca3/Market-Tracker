using System.ComponentModel.DataAnnotations;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Domain.Models.Market.Inventory.Product;

namespace market_tracker_webapi.Application.Http.Models.Product;

public class ProductCreationInputModel
{
    [Required] public string Id { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "Name too long.")]
    public string Name { get; set; }

    [Required] public string ImageUrl { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public int Quantity { get; set; } = 1;

    [Required] public ProductUnit Unit { get; set; } = ProductUnit.Units;

    [Required] public string BrandName { get; set; }

    [Required] public int? CategoryId { get; set; }

    [Required] public int? Price { get; set; }

    [Range(1, 100, ErrorMessage = "Promotion percentage must be between 1 and 100.")]
    public int? PromotionPercentage { get; set; }
}