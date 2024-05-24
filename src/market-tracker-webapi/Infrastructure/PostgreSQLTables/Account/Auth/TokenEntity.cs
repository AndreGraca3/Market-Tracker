using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.Account.Auth
{
    [Table("token", Schema = "MarketTracker")]
    public class TokenEntity
    {
        [Key] [Column("token_value")] public Guid TokenValue { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; } = DateTime.Now;

        [Column("expires_at")]
        public required DateTime ExpiresAt { get; set; }

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