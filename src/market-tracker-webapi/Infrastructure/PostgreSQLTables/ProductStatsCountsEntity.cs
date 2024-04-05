using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("product_stats_counts")]
public class ProductStatsCountsEntity
{
    [Key]
    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("favourites")]
    public int Favourites { get; set; }

    [Column("ratings")]
    public int Ratings { get; set; }

    [Column("lists")]
    public int Lists { get; set; }

    // public double AverageRating { get; set; }
}
