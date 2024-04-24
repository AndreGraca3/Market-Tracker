using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables
{
    [Table("token", Schema = "MarketTracker")]
    public class TokenEntity
    {
        [Key] [Column("token_value")] public Guid TokenValue { get; set; }

        [Column("created_at")] public DateTime CreatedAt { get; }

        [Column("expires_at")] public DateTime ExpiresAt { get; set; }

        [Column("user_id")] public required Guid UserId { get; set; }

        public Token ToToken()
        {
            return new Token
            (
                TokenValue,
                CreatedAt,
                ExpiresAt,
                UserId
            );
        }
    }
}