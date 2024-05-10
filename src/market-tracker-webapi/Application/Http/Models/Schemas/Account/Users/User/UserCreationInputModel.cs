namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.User;

public record UserCreationInputModel(string Name, string Email, string Password, string Role);