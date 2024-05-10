namespace market_tracker_webapi.Application.Domain.Models.Market.Retail.Sales.Pricing;

public record PriceAlert(string Id, Guid ClientId, string ProductId, int PriceThreshold, DateTime CreatedAt);
