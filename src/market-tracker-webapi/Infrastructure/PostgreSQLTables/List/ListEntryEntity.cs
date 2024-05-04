using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.List;

[Table("list_entry", Schema = "MarketTracker")]
public class ListEntryEntity
{
    
    [Column("list_id")]
    public int ListId { get; set; }
    
    [Column("product_id")]
    public required string ProductId { get; set; }
    
    [Column("store_id")]
    public int? StoreId { get; set; }
    
    [Column("quantity")]
    public int Quantity { get; set; }
    
    public ListEntry ToListEntry()
    {
        return new ListEntry()
        {
            ListId = ListId,
            ProductId = ProductId,
            StoreId = StoreId,
            Quantity = Quantity
        };
    }
}