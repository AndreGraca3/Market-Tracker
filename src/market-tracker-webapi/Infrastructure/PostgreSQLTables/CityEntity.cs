using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

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
