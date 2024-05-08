using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Domain.Models.Market.Price;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.Market;

[Table("promotion", Schema = "MarketTracker")]
public class PromotionEntity
{
    [Column("percentage")] public required int Percentage { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Key] [Column("price_entry_id")] public required string PriceEntryId { get; set; }

    public Promotion ToPromotion()
    {
        return new Promotion(Percentage, CreatedAt);
    }
}