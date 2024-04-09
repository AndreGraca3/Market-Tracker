using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Dto.Price;

public record PriceInfo(int Price, Promotion? Promotion, DateTime CreatedAt)
{
    public static PriceInfo Calculate(int oldPrice, Promotion? promotion, DateTime createdAt)
    {
        var effectivePrice = promotion?.CalculatePrice(oldPrice) ?? oldPrice;
        return new PriceInfo(effectivePrice, promotion, createdAt);
    }
}
