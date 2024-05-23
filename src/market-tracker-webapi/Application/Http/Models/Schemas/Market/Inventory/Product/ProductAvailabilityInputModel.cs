using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product;

public record ProductAvailabilityInputModel([Required] bool IsAvailable);