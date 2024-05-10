using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product;

public class ProductAvailabilityInputModel
{
    [Required] public bool IsAvailable { get; set; }
}