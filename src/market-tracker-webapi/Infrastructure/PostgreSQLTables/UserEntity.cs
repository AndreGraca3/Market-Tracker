using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables
{
    [Table("user", Schema = "MarketTracker")]
    public class UserEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("username")] public required string Username { get; set; }

        [Column("name")] public required string Name { get; set; }

        [Column("email")] public required string Email { get; set; }

        [Column("password")] public required string Password { get; set; }
        
        [DataType(DataType.Date)]
        [Column("created_at")] public readonly DateTime CreatedAt;
    }
}