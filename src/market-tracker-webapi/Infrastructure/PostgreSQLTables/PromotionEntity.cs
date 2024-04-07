using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("promotion")]
public class PromotionEntity
{
    [Column("id")]
    public int Id { get; set; }

    [Column("percentage")]
    public int percentage { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    public Promotion ToPromotion()
    {
        return new Promotion(Id, percentage, CreatedAt);
    }
}
