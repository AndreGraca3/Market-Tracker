using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("price_entry", Schema = "MarketTracker")]
public class PriceEntryEntity
{
    [Key] [Column("id")] public string Id { get; set; } = RandomStringGenerator.GenerateRandomString(25);

    [Column("price")] public required int Price { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("store_id")] public int StoreId { get; set; }

    [Column("product_id")] public required string ProductId { get; set; }
}