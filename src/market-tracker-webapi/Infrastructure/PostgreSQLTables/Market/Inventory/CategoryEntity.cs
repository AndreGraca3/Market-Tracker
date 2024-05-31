using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory;

[Table("category", Schema = "MarketTracker")]
public class CategoryEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public required string Name { get; set; }

    public Category ToCategory()
    {
        return new Category(Id, Name);
    }
}
