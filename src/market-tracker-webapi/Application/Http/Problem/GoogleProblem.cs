using market_tracker_webapi.Application.Service.Errors.Google;

namespace market_tracker_webapi.Application.Http.Problem;

public class GoogleProblem(
    int status,
    string subType,
    string title,
    string detail,
    object? data = null
) : Problem(status, subType, title, detail, data)
{
    public class InvalidIssuer(GoogleTokenCreationError.InvalidIssuer issuerName) : UserProblem(
        401,
        "invalid-issuer",
        "Invalid issuer",
        $"The token provided was not issued by {issuerName}"
    );
    
    public class InvalidTokenFormat() : UserProblem(
        401,
        "invalid-format",
        "Invalid format",
        "The token provided is not a Json Web Token"
    );
}