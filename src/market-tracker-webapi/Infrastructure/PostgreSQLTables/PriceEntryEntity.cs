using System.ComponentModel.DataAnnotations.Schema;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("price_entry")]
public class PriceEntryEntity
{
    [Column("price")]
    public int Price { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("promotion_id")]
    public int? PromotionId { get; set; }
}
