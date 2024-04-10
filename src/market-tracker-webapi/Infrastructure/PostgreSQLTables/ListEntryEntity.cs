using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("list_product", Schema = "MarketTracker")]
public class ListEntryEntity
{
    [Key]
    [Column("list_id")]
    public int ListId { get; set; }
    
    [Key]
    [Column("product_id")]
    public required string ProductId { get; set; }
    
    [Key]
    [Column("store_id")]
    public int StoreId { get; set; }
    
    [Column("quantity")]
    public int Quantity { get; set; }
    
    public ProductInList ToProductInList()
    {
        return new ProductInList()
        {
            ListId = ListId,
            ProductId = ProductId,
            StoreId = StoreId,
            Quantity = Quantity
        };
    }
}