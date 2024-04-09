using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("price_entry", Schema = "MarketTracker")]
public class PriceEntryEntity
{
    [Column("id")]
    public string Id { get; set; }

    [Column("price")]
    public int Price { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("store_id")]
    public int StoreId { get; set; }

    [Column("product_id")]
    public string ProductId { get; set; }
}
