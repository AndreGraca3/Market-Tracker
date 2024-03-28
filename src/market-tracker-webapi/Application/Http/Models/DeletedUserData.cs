namespace market_tracker_webapi.Application.Models
{
    public class DeletedUserData
    {
        public Guid Id { get; set; }

        public required DateTime CreatedAt { get; set; }
    }
}