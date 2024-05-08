using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Domain.Models.Account;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.Account;

[Table("fcm_registration", Schema = "MarketTracker")]
[PrimaryKey(nameof(ClientId), nameof(DeviceId))]
public class FcmRegisterEntity
{
    [Column("client_id")]
    public required Guid ClientId { get; set; }
    
    [Column("device_id")]
    public required string DeviceId { get; set; }
    
    [Column("token")]
    public required string Token { get; set; }

    [Column("updated_at")] public DateTime UpdatedAt { get; set; } = DateTime.Now;
    
    public DeviceToken ToDeviceToken()
    {
        return new DeviceToken(ClientId, DeviceId, Token);
    }
}