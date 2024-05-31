using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product;

public record PriceAlertCreationInputModel(
    [Required] string ProductId,
    [Required] int StoreId,
    [Required] int PriceThreshold
);