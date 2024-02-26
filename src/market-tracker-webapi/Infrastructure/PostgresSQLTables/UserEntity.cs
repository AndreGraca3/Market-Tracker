using System.ComponentModel.DataAnnotations.Schema;

namespace market_tracker_webapi.Infrastructure.PostgresSQLTables
{
    [Table("User", Schema = "MarketTracker")]
    public class UserEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string Name { get; set; }

        // public required string Password { get; set; }

        // public required string Email { get; set; }

        // public required string Avatar { get; set; }
    }
}
