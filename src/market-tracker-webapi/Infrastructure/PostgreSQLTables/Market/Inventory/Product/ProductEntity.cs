using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory.Product;

using Product = Application.Domain.Schemas.Market.Inventory.Product.Product;

[Table("product", Schema = "MarketTracker")]
public class ProductEntity
{
    [Key] [Column("id")] public required string Id { get; set; }

    [Column("name")] public required string Name { get; set; }

    [Column("image_url")] public required string ImageUrl { get; set; }

    [Column("quantity")] public required int Quantity { get; set; }

    [DefaultValue("unidades")]
    [Column("unit")]
    public required string Unit { get; set; }

    [DefaultValue(0)] [Column("views")] public int Views { get; set; }

    [DefaultValue(0)] [Column("rating")] public double Rating { get; set; }

    [Column("brand_id")] public required int BrandId { get; set; }

    [Column("category_id")] public required int CategoryId { get; set; }

    public Product ToProduct(Brand brand, Category category)
    {
        return new Product(
            Id,
            Name,
            ImageUrl,
            Quantity,
            Unit.GetUnitFromName(),
            Views,
            Rating,
            brand,
            category
        );
    }

    public ProductItem ToProductItem(Brand brand)
    {
        return new ProductItem(
            Id,
            Name,
            ImageUrl,
            brand.Name
        );
    }
}