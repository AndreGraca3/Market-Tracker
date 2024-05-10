namespace market_tracker_webapi.Application.Http.Controllers.Account;

public record AuthenticatedUser(Domain.Models.Account.Users.User User, Domain.Models.Account.Auth.Token Token);
