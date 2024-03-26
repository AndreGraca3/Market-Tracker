namespace market_tracker_webapi.Application.Models
{
    public class UserData
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }
        
        public required string Username { get; set; }
        
        public required string Email { get; set; }

        public required string Password { get; set; }
        
        public required DateTime CreatedAt { get; set; }
    }
}
