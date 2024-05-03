namespace market_tracker_webapi.Application.Repository.Dto.User;

public record UserDetails(Guid Id, string Username, string Name, string Email, string Role, DateTime CreatedAt);