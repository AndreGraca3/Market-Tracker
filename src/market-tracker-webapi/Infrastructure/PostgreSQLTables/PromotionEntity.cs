using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("promotion", Schema = "MarketTracker")]
public class PromotionEntity
{
    [Column("percentage")]
    public int percentage { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Key]
    [Column("price_entry_id")]
    public string PriceEntryId { get; set; }

    public Promotion ToPromotion(int oldPrice)
    {
        return new Promotion(percentage, oldPrice, CreatedAt, PriceEntryId);
    }
}
