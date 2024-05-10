namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Client;

public record ClientCreationInputModel(
    string Username,
    string Name,
    string Email,
    string Password,
    string? Avatar
);