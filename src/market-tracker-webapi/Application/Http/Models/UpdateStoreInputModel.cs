﻿namespace market_tracker_webapi.Application.Http.Models;

public class UpdateStoreInputModel
{
    public string? Name { get; set; }
    
    public string? Address { get; set; }
    
    public int CityId { get; set; }
    
    public int CompanyId { get; set; }
}