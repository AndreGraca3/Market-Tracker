namespace market_tracker_webapi.Application.Service.Results;

public class ProductCreationResult
{
    public required string Id { get; set; }

    public required bool IsNew { get; set; }

    public required bool PriceChanged { get; set; }
}