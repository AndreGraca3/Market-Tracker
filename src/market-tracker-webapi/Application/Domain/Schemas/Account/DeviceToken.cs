namespace market_tracker_webapi.Application.Domain.Schemas.Account;

public record DeviceToken(Guid ClientId, string DeviceId, string Token);