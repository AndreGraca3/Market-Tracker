namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.User;

public record UsersOutputModel(IEnumerable<UserOutputModel> Users, int Total);