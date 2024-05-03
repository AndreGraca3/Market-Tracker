using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("product_stats_counts", Schema = "MarketTracker")]
public class ProductStatsCountsEntity
{
    [Key]
    [Column("product_id")]
    public string ProductId { get; set; }

    [Column("favourites")]
    public int Favourites { get; set; }

    [Column("ratings")]
    public int Ratings { get; set; }
}
