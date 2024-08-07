using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.Market;

[Table("price_entry", Schema = "MarketTracker")]
public class PriceEntryEntity
{
    [Key] [Column("id")] public string Id { get; set; } = RandomStringGenerator.GenerateRandomString(25);

    [Column("price")] public required int Price { get; set; }

    [Column("created_at")] public required DateTime CreatedAt { get; set; }

    [Column("store_id")] public int StoreId { get; set; }

    [Column("product_id")] public required string ProductId { get; set; }

    public ProductHistoryPrice ToProductHistoryPrice()
    {
        return new ProductHistoryPrice(
            CreatedAt,
            Price
        );
    }
}