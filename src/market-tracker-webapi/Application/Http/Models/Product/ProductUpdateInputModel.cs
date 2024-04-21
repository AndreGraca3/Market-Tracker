using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Product;

public class ProductUpdateInputModel
{
    [MaxLength(100, ErrorMessage = "Name too long.")]
    public string? Name { get; set; }

    public string? ImageUrl { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public int? Quantity { get; set; }

    [RegularExpression(
        "^(unidades|kilogramas|gramas|litros|mililitros)$",
        ErrorMessage = "Wrong unit provided. Must be 'unidades', 'kilogramas', 'gramas', 'litros' or 'mililitros'."
    )]
    public string? Unit { get; set; }

    public string? BrandName { get; set; }

    public int? CategoryId { get; set; }
}
