using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory.Product;

[Table("product_availability", Schema = "MarketTracker")]
[PrimaryKey("ProductId", "StoreId")]
public class ProductAvailabilityEntity
{
    [Column("is_available")] public required bool IsAvailable { get; set; }

    [Column("last_checked")] public required DateTime LastChecked { get; set; }

    [Column("product_id")] public required string ProductId { get; set; }

    [Column("store_id")] public required int StoreId { get; set; }

    public StoreAvailability ToStoreAvailability()
    {
        return new StoreAvailability(
            StoreId,
            ProductId,
            IsAvailable,
            LastChecked
        );
    }
}