using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Domain.Models.Market.Store;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.Market;

[Table("city", Schema = "MarketTracker")]
public class CityEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("name")]
    public required string Name { get; set; }

    public City ToCity()
    {
        return new City { Id = Id, Name = Name };
    }
}
