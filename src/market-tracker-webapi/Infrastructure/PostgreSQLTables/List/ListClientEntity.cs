using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain.Schemas.List;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.List;

[Table("list_client", Schema = "MarketTracker")]
public class ListClientEntity
{
    [Column("client_id")] public required Guid ClientId { get; set; }

    [Column("list_id")] public required string ListId { get; set; }

    public ListClient ToListClient()
    {
        return new ListClient(
            ClientId,
            ListId
        );
    }
}