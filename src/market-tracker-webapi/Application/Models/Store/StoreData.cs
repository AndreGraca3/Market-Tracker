namespace market_tracker_webapi.Application.Models
{
    public class StoreData
    {
        public int Id { get; set; }
        public required string Address { get; set; }
        
        public required int? CityId { get; set; }
        
        public int CompanyId { get; set; }
    }
}
