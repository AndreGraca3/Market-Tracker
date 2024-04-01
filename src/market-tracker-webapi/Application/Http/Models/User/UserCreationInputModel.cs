namespace market_tracker_webapi.Application.Http.Models.User;

public record UserCreationInputModel(string Username, string Name, string Email, string Password);