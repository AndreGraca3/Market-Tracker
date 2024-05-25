using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory.Product;

[Table("product_favourite", Schema = "MarketTracker")]
[PrimaryKey("ClientId", "ProductId")]
public class ProductFavouriteEntity
{
    [Column("client_id")]
    public Guid ClientId { get; set; }

    [Column("product_id")]
    public string ProductId { get; set; }
}
