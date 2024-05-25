using market_tracker_webapi.Application.Domain.Schemas.Account.Users;

namespace market_tracker_webapi.Application.Domain.Schemas.Account.Auth;

public record AuthenticatedUser(User User, Token Token);
