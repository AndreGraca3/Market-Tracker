namespace market_tracker_webapi.Application.Domain;

public record Promotion(int Percentage, int OldPrice, DateTime CreatedAt)
{
    public int CalculatePrice(int oldPrice)
    {
        return oldPrice - oldPrice * Percentage / 100;
    }
}