using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Domain.Models.Market.Retail.Sales.Pricing;

public record Price(int BasePrice, int FinalPrice, Promotion? Promotion, DateTime CreatedAt)
{
    public Price(int basePrice, Promotion? promotion, DateTime createdAt) : this(
        basePrice,
        basePrice.ApplyPercentage(promotion?.Percentage),
        promotion,
        createdAt
    )
    {
    }
}