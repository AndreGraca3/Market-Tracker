namespace market_tracker_webapi.Application.Http.Models;

public record AuthenticatedUser(Domain.User User, Domain.Token Token);
