using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Domain.Models.List;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.List;

[Table("list", Schema = "MarketTracker")]
public class ListEntity
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("name")] public required string Name { get; set; }

    [Column("archived_at")] public DateTime? ArchivedAt { get; set; } = null;

    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("owner_id")] public required Guid OwnerId { get; set; }

    public ShoppingList ToShoppingList()
    {
        return new ShoppingList(
            Id,
            Name,
            ArchivedAt,
            CreatedAt,
            OwnerId
        );
    }
}