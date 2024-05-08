namespace market_tracker_webapi.Application.Http.Models;

public record AuthenticatedUser(Domain.Models.Account.Users.User User, Domain.Models.Account.Auth.Token Token);
