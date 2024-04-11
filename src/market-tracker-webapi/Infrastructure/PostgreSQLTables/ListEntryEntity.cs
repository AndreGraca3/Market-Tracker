using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("list_product", Schema = "MarketTracker")]
public class ListEntryEntity
{
    
    [Column("list_id")]
    public int ListId { get; set; }
    
    [Column("product_id")]
    public required string ProductId { get; set; }
    
    [Column("store_id")]
    public int StoreId { get; set; }
    
    [Column("quantity")]
    public int Quantity { get; set; }
    
    public ListEntry ToProductInList()
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