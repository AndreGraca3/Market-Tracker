using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Domain.Models.Market.Inventory;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory;

[Table("brand", Schema = "MarketTracker")]
public class BrandEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public required string Name { get; set; }

    public Brand ToBrand()
    {
        return new Brand(this.Id, this.Name);
    }
}
