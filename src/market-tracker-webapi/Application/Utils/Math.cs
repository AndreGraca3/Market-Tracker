namespace market_tracker_webapi.Application.Utils;

public static class Math
{
    public static int ApplyPercentage(this int value, int? percentage) =>
        percentage is null ? value : value - value * percentage.Value / 100;
}