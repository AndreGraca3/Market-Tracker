namespace market_tracker_webapi.Application.Http.Models;

public record Pagination(int Skip, int Limit)
{
    public const int MaxLimit = 50;
};