namespace market_tracker_webapi.Application.Domain.Models.Market.Retail.Shop;

public class Company
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required DateTime CreatedAt { get; set; }
}