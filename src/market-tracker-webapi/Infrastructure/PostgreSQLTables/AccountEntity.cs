using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("account", Schema = "MarketTracker")]
public class AccountEntity
{
    [Key] [Column("user_id")] public required Guid UserId { get; set; }

    [Column("password")] public required string Password { get; set; }

    public Account ToAccount()
    {
        return new Account(
            UserId,
            Password
        );
    }
}