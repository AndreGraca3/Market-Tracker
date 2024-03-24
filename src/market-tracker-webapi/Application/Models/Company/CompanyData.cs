namespace market_tracker_webapi.Application.Models.Company;

public class CompanyData
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
        
    public required DateTime CreatedAt { get; set; }
}