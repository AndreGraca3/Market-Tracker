using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("price_alert", Schema = "MarketTracker")]
[PrimaryKey("ClientId", "ProductId")]
public class PriceAlertEntity
{
    [Column("client_id")]
    public required Guid ClientId { get; set; }

    [Column("product_id")]
    public required string ProductId { get; set; }

    [Column("price_threshold")]
    public required int PriceThreshold { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    public PriceAlert ToPriceAlert()
    {
        return new PriceAlert(ClientId, ProductId, PriceThreshold, CreatedAt);
    }
}
