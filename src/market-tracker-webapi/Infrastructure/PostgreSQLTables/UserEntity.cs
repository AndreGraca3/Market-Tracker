using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables
{
    [Table("user", Schema = "MarketTracker")]
    public class UserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("name")] public required string Name { get; set; }

        // public required string Password { get; set; }

        // public required string Email { get; set; }

        // public required string Avatar { get; set; }
    }
}