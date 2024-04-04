using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Product;

public record ProductCreationInputModel(
    [Required] int? Id,
    [Required] [MaxLength(50, ErrorMessage = "Name too long.")] string Name,
    [Required] string ImageUrl,
    [Required] int? Quantity,
    [RegularExpression(
        "^(unidades|kilogramas|gramas|litros|mililitros)$",
        ErrorMessage = "Wrong unit provided. Must be 'unidades', 'kilogramas', 'gramas', 'litros' or 'mililitros'."
    )]
        string Unit,
    [Required] string BrandName,
    [Required] int? CategoryId
);
