using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Product;

public record PriceAlertCreationInputModel(
    [Required] string ProductId,
    [Required] int StoreId,
    [Required] int PriceThreshold
);