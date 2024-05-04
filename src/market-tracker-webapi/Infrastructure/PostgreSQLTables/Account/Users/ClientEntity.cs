using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.Account.Users;

[Table("client", Schema = "MarketTracker")]
public class ClientEntity
{
    [Key] [Column("id")] public required Guid UserId { get; set; }

    [Column("username")] public required string Username { get; set; }

    [Column("avatar_url")] public required string? Avatar { get; set; }

    public Client ToClient()
    {
        return new Client(
            UserId,
            Username,
            Avatar
        );
    }
}