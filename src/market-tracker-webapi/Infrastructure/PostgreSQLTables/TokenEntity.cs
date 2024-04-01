using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables
{
    [Table("token", Schema = "MarketTracker")]
    public class TokenEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("token_value")]
        public Guid TokenValue { get; set; }

        [Column("created_at")] public required DateTime CreatedAt { get; set; }

        [Column("expires_at")] public required DateTime ExpiresAt { get; set; }

        [Column("user_id")] public required Guid UserId { get; set; }
    }
}