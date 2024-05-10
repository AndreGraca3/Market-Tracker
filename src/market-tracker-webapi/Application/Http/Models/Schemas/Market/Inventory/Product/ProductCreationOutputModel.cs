namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product;

public class ProductCreationOutputModel
{
    public required string Id { get; set; }

    public required bool IsNew { get; set; }

    public required bool PriceChanged { get; set; }
}