namespace market_tracker_webapi.Application.Domain;

public class CompanyDomain
{
    public required int Id { get; set; }
    
    public required string Name { get; set; }
        
    public required DateTime CreatedAt { get; set; }
}