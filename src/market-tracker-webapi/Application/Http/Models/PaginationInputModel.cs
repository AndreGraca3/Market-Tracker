﻿namespace market_tracker_webapi.Application.Http.Models;

public record PaginationInputModel(int Skip, int Limit)
{
    public const int MaxLimit = 50;
};