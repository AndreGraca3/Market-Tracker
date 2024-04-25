namespace market_tracker_webapi.Application.Http.Models.Client;

public record ClientCreationInputModel(
    string Username,
    string Name,
    string Email,
    string Password,
    string? Avatar
);