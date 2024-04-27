namespace market_tracker_webapi.Application.Http.Models.Product;

public class ProductCreationOutputModel
{
    public required string Id { get; set; }

    public required bool IsNew { get; set; }

    public required bool PriceChanged { get; set; }
}