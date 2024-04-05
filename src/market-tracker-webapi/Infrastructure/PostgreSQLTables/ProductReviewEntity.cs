using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("product_review", Schema = "MarketTracker")]
[PrimaryKey(nameof(ClientId), nameof(ProductId))]
public class ProductReviewEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("product_id")]
    public required int ProductId { get; set; }

    [Column("client_id")]
    public required Guid ClientId { get; set; }

    [Column("rating")]
    public required int Rating { get; set; }

    [Column("text")]
    [MaxLength(255)]
    public string? Text { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    public ProductReview ToProductReview()
    {
        return new ProductReview(Id, ClientId, ProductId, Rating, Text, CreatedAt);
    }
}
