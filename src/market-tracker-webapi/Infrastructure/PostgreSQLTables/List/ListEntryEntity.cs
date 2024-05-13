using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain.Schemas.List;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.List;

[Table("list_entry", Schema = "MarketTracker")]
public class ListEntryEntity
{
    [Key] [Column("id")] public string Id { get; } = RandomStringGenerator.GenerateRandomString(25);

    [Column("list_id")] public string ListId { get; set; }

    [Column("product_id")] public required string ProductId { get; set; }

    [Column("store_id")] public int? StoreId { get; set; }

    [Column("quantity")] public int Quantity { get; set; }

    public ListEntry ToListEntry(Product product)
    {
        return new ListEntry(
            new ListEntryId(Id),
            product,
            StoreId,
            Quantity
        );
    }
}