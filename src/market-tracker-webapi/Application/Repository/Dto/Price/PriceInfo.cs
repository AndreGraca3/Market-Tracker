using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Dto.Price;

public record PriceInfo(int RegularPrice, int FinalPrice, Promotion? Promotion, DateTime CreatedAt)
{
    public static PriceInfo Calculate(int regularPrice, Promotion? promotion, DateTime createdAt)
    {
        var finalPrice = promotion?.CalculatePrice(regularPrice) ?? regularPrice;
        return new PriceInfo(regularPrice, finalPrice, promotion, createdAt);
    }
}
