namespace market_tracker_webapi.Application.Http.Models;

using DUser = Domain.User;
using DToken = Domain.Token;

public record AuthenticatedUser(
    DUser User,
    DToken Token
);