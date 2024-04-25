using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("client", Schema = "MarketTracker")]
public class ClientEntity
{
    [Key] [Column("user_id")] public required Guid UserId { get; set; }

    [Column("avatar_url")] public required string Avatar { get; set; }

    public Client ToClient()
    {
        return new Client(
            UserId,
            Avatar
        );
    }
}