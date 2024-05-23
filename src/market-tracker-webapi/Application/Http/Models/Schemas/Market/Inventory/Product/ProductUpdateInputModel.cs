using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product;

public record ProductUpdateInputModel(
    [MaxLength(100, ErrorMessage = "Name too long.")]
    string? Name,
    string? ImageUrl,
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    int? Quantity,
    [RegularExpression(
        "^(unidades|kilogramas|gramas|litros|mililitros)$",
        ErrorMessage = "Wrong unit provided. Must be 'unidades', 'kilogramas', 'gramas', 'litros' or 'mililitros'."
    )]
    string? Unit,
    string? BrandName,
    int? CategoryId
);