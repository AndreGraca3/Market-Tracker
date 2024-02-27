using System.ComponentModel.DataAnnotations.Schema;

namespace market_tracker_webapi.Infrastructure.PostgresSQLTables
{
    [Table("user", Schema = "MarketTracker")]
    public class UserEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public required string Name { get; set; }

        // public required string Password { get; set; }

        // public required string Email { get; set; }

        // public required string Avatar { get; set; }
    }
}
