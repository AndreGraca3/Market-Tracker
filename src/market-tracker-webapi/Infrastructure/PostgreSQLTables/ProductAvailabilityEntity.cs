using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("product_availability", Schema = "MarketTracker")]
[PrimaryKey("ProductId", "StoreId")]
public class ProductAvailabilityEntity
{
    [Column("is_available")]
    public required bool IsAvailable { get; set; }

    [Column("last_checked")]
    public DateTime LastChecked { get; set; } = DateTime.Now;

    [Column("product_id")]
    public required string ProductId { get; set; }

    [Column("store_id")]
    public required int StoreId { get; set; }
}
