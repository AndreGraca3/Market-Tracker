using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Product;

public class ProductAvailabilityInputModel
{
    [Required] public bool IsAvailable { get; set; }
}