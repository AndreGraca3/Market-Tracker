namespace market_tracker_webapi.Application.Repository.Dto.Client;

public record ClientInfo(
    Guid Id,
    string Username,
    string Name,
    string Email,
    DateTime CreatedAt,
    string AvatarUrl
);