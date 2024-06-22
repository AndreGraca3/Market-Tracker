using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory.Product;

[Table("product_review", Schema = "MarketTracker")]
public class ProductReviewEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("product_id")] public required string ProductId { get; set; }

    [Column("client_id")] public required Guid ClientId { get; set; }

    [Column("rating")] public required int Rating { get; set; }

    [Column("text")] public string? Text { get; set; }

    [Column("created_at")] public required DateTime CreatedAt { get; init; }

    public ProductReview ToProductReview(ClientItem client)
    {
        return new ProductReview(Id, client, ProductId, Rating, Text, CreatedAt);
    }
}