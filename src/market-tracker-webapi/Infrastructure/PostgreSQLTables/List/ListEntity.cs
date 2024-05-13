using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain.Schemas.List;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.List;

[Table("list", Schema = "MarketTracker")]
public class ListEntity
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = RandomStringGenerator.GenerateRandomString(25);

    [Column("name")] public required string Name { get; set; }

    [Column("archived_at")] public DateTime? ArchivedAt { get; set; } = null;

    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("owner_id")] public required Guid OwnerId { get; set; }

    public ShoppingList ToShoppingList(IEnumerable<Guid> memberIds)
    {
        return new ShoppingList(
            Id,
            Name,
            ArchivedAt,
            CreatedAt,
            OwnerId,
            memberIds
        );
    }

    public ShoppingListItem ToShoppingListItem()
    {
        return new ShoppingListItem(
            Id,
            Name,
            ArchivedAt,
            CreatedAt,
            OwnerId
        );
    }
}