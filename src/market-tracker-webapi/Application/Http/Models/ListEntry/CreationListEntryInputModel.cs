﻿namespace market_tracker_webapi.Application.Http.Models.ListEntry;

public class CreationListEntryInputModel
{
    public int ListId { get; set; }
    public required string ProductId { get; set; }
    public int StoreId { get; set; }
    public int Quantity { get; set; }
}