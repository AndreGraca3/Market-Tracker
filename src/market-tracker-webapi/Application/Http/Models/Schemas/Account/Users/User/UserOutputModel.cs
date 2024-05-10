namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.User;

public record UserOutputModel(Guid Id, string Name, string Role, DateTime CreatedAt);