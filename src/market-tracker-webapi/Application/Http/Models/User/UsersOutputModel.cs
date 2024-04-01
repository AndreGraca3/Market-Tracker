namespace market_tracker_webapi.Application.Http.Models.User;

public record UsersOutputModel(IEnumerable<UserOutputModel> Users, int Total);