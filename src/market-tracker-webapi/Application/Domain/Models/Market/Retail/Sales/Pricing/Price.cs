using market_tracker_webapi.Application.Domain.Models.Market.Price;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Domain.Models.Market.Retail.Sales.Pricing;

public record Price(int RegularPrice, int FinalPrice, Promotion? Promotion, DateTime CreatedAt)
{
    public Price(int regularPrice, Promotion? promotion, DateTime createdAt) : this(
        regularPrice,
        regularPrice.ApplyPercentage(promotion?.Percentage),
        promotion,
        createdAt
    )
    {
    }
}