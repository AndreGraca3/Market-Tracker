namespace market_tracker_webapi.Application.Http.Models.Client;

public record ClientOutputModel(
    Guid Id,
    string Username,
    string Name,
    string Email,
    DateTime CreatedAt,
    string? AvatarUrl);