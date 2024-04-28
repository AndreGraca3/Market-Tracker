using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("fcm_registration", Schema = "MarketTracker")]
[PrimaryKey(nameof(ClientId), nameof(DeviceId))]
public class FcmRegisterEntity
{
    [Column("client_id")]
    public required Guid ClientId { get; set; }
    
    [Column("device_id")]
    public required string DeviceId { get; set; }
    
    [Column("firebase_token")]
    public required string FirebaseToken { get; set; }
}