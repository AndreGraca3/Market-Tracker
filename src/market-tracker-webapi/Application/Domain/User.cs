namespace market_tracker_webapi.Application.Domain;

public record User(
    Guid Id,
    string Username,
    string Name,
    string Email,
    string Role,
    DateTime CreatedAt
);
