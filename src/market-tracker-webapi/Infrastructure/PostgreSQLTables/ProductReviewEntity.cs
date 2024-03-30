using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("product_review", Schema = "MarketTracker")]
[PrimaryKey(nameof(ClientId), nameof(ProductId))]
public class ProductReviewEntity
{
    [Column("client_id")] public Guid ClientId { get; set; }

    [Column("product_id")] public int ProductId { get; set; }

    [Column("rate")] public int Rate { get; set; }

    [Column("comment")] public string Comment { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; set; }

    public ProductReview ToProductReview()
    {
        return new ProductReview(ClientId, ProductId, Rate, Comment, CreatedAt);
    }
}