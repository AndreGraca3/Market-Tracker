using System.Diagnostics.CodeAnalysis;

namespace market_tracker_webapi.Application.Utils;

[ExcludeFromCodeCoverage]
public static class MathUtils
{
    public static int ApplyPercentage(this int value, int? percentage) {
        var promotionDecimal = percentage is null ? 0 : percentage.Value / 100.0;

        // Calculate discounted value
        var discountedPrice = value * (1 - promotionDecimal);

        // Round to the nearest integer
        return (int)Math.Round(discountedPrice);
    }
}
