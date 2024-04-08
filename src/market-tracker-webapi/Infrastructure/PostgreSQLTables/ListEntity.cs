using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("list", Schema = "MarketTracker")]
public class ListEntity
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Column("client_id")]
    public required Guid ClientId { get; set; }
    
    [Column("name")]
    public required string Name { get; set; }
    
    [Column("archived_at")]
    public DateTime? ArchivedAt { get; set; }
    
    public ListOfProducts ToListOfProducts()
    {
        return new ListOfProducts
        {
            Id = Id,
            ClientId = ClientId,
            ListName = Name,
            ArchivedAt = ArchivedAt
        };
    }
}