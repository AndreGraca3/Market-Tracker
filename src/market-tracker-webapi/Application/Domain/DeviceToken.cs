namespace market_tracker_webapi.Application.Domain;

public class DeviceToken(Guid clientId, string deviceId, string token)
{
    public Guid ClientId { get; } = clientId;
    public string DeviceId { get; } = deviceId;
    public string Token { get; } = token;
}