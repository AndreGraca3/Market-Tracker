using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.Account;

[Table("account", Schema = "MarketTracker")]
public class AccountEntity
{
    [Key] [Column("user_id")] public required Guid UserId { get; set; }

    [Column("password")] public required string Password { get; set; }

    public Application.Domain.Account ToAccount()
    {
        return new Application.Domain.Account(
            UserId,
            Password
        );
    }
}