using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.List;

[Table("list_client", Schema = "MarketTracker")]
public class ListClientEntity
{
    [Column("client_id")]
    public required Guid ClientId { get; set; }
    
    [Column("list_id")]
    public required int ListId { get; set; }
    
    public ListClient ToListClient()
    {
        return new ListClient
        {
            ClientId = ClientId,
            ListId = ListId
        };
    }
}