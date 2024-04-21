using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("product_availability", Schema = "MarketTracker")]
[PrimaryKey("ProductId", "StoreId")]
public class ProductAvailabilityEntity
{
    [Column("is_available")]
    public bool IsAvailable { get; set; }

    [Column("last_checked")]
    public DateTime LastChecked { get; set; }

    [Column("product_id")]
    public string ProductId { get; set; }

    [Column("store_id")]
    public int StoreId { get; set; }
}
