﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Store;

using Store = Application.Domain.Schemas.Market.Retail.Shop.Store;

[Table("store", Schema = "MarketTracker")]
public class StoreEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")] public required string Name { get; set; }

    [Column("address")] public required string Address { get; set; }

    [ForeignKey("city")]
    [Column("city_id")]
    public int? CityId { get; set; }

    [ForeignKey("company")]
    [Column("company_id")]
    public int CompanyId { get; set; }

    [ForeignKey("operator")]
    [Column("operator_id")]
    public required Guid OperatorId { get; set; }

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
            new StoreId(Id),
            Name,
            Address,
            CityId,
            CompanyId,
            OperatorId
        );
    }
}