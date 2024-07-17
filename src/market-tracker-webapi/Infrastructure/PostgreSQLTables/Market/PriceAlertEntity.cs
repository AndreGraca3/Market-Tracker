using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales.Pricing;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.Market;

[Table("price_alert", Schema = "MarketTracker")]
public class PriceAlertEntity
{
    [Key]
    [Column("id")]
    [StringLength(25)]
    public string Id { get; init; } = RandomStringGenerator.GenerateRandomString(25);

    [Column("client_id")] public required Guid ClientId { get; set; }

    [Column("product_id")] public required string ProductId { get; set; }

    [Column("store_id")] public required int StoreId { get; set; }

    [Column("price_threshold")] public required int PriceThreshold { get; set; }

    [Column("created_at")] public required DateTime CreatedAt { get; set; }

    public PriceAlert ToPriceAlert(string productName, string productImageUrl, string storeName)
    {
        return new PriceAlert(
            new PriceAlertId(Id),
            ClientId,
            new PriceAlertProduct(ProductId, productName, productImageUrl),
            new PriceAlertStore(StoreId, storeName),
            PriceThreshold,
            CreatedAt
        );
    }
}