using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("price_entry")]
[PrimaryKey("ProductId", "StoreId", "CreatedAt")]
public class PriceEntryEntity
{
    [Column("price")]
    public int Price { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("store_id")]
    public int StoreId { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("promotion_id")]
    public int? PromotionId { get; set; }
}
