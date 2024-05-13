namespace market_tracker_webapi.Application.Domain;

public record Promotion(int Percentage, DateTime CreatedAt)
{
    public int CalculatePrice(int oldPrice)
    {
        return oldPrice - oldPrice * Percentage / 100;
    }
}