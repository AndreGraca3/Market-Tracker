using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("store", Schema = "MarketTracker")]
public class StoreEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public required string Name { get; set; }

    [Required]
    [StringLength(200)]
    [Column("address")]
    public required string Address { get; set; }

    [StringLength(30)]
    [Column("city_id")]
    public int? CityId { get; set; }

    [Column("company_id")]
    public int CompanyId { get; set; }
    
    [Column("operator_id")]
    public Guid OperatorId { get; set; }
    
    public Store ToStore()
    {
        return new Store
        {
            Id = Id,
            Name = Name,
            Address = Address,
            CityId = CityId,
            CompanyId = CompanyId,
            OperatorId = OperatorId
        };
    }
}
