using System.ComponentModel.DataAnnotations.Schema;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables
{
    [Table("product", Schema = "MarketTracker")]
    public class ProductEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] [Column("id")]
        public readonly int Id;

        [Column("name")] public required string Name { get; set; }
    }
}