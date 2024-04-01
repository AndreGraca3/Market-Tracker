namespace market_tracker_webapi.Application.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        // public required string Email { get; set; }

        // public required string Password { get; set; }

        // public required string Avatar { get; set; }
    }
}
