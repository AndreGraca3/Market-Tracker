namespace market_tracker_webapi.Application.Domain
{
    public class Store
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        public required string Address { get; set; }

        public required int? CityId { get; set; }

        public int CompanyId { get; set; }
    }
}
