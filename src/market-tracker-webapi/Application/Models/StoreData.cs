namespace market_tracker_webapi.Application.Models
{
    public class StoreData
    {
        public int Id { get; set; }
        public required string Address { get; set; }
        
        public required string City { get; set; }
        
        public DateTime? OpenTime { get; set; }
        
        public DateTime? CloseTime { get; set; }
        
        public int CompanyId { get; set; }
    }
}
