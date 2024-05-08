namespace market_tracker_webapi.Application.Domain.Models.Account;

public record DeviceToken(Guid ClientId, string DeviceId, string Token);