using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain.Models.Market.Retail.Shop;
using market_tracker_webapi.Application.Domain.Models.Market.Store;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Store;

using Store = Application.Domain.Models.Market.Retail.Shop.Store;

[Table("store", Schema = "MarketTracker")]
public class StoreEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")] public required string Name { get; set; }

    [Required]
    [StringLength(200)]
    [Column("address")]
    public required string Address { get; set; }

    [StringLength(30)]
    [ForeignKey("city")]
    [Column("city_id")]
    public int? CityId { get; set; }

    [ForeignKey("company")]
    [Column("company_id")]
    public int CompanyId { get; set; }

    [ForeignKey("operator")]
    [Column("operator_id")]
    public Guid OperatorId { get; set; }

    public Store ToStore(City? city, Company company)
    {
        return new Store(
            Id,
            Name,
            Address,
            city,
            company,
            OperatorId
        );
    }

    public StoreItem ToStoreItem()
    {
        return new StoreItem(
            Id,
            Name,
            Address,
            CityId,
            CompanyId,
            OperatorId
        );
    }
}