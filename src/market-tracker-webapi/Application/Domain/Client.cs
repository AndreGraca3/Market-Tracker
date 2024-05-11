namespace market_tracker_webapi.Application.Domain;

public record Client(
    Guid Id,
    string Username,
    string AvatarUrl
);