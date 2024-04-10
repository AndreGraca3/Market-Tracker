﻿namespace market_tracker_webapi.Application.Domain;

public class ProductInList
{
    public int ListId { get; set; }
    
    public required string ProductId { get; set; }
    
    public int StoreId { get; set; }
    
    public int Quantity { get; set; }
}