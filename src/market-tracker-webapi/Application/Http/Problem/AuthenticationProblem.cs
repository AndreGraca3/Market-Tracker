namespace market_tracker_webapi.Application.Http.Problem;

public class AuthenticationProblem(
    int status,
    string subtype,
    string title,
    string detail,
    object? data = null
) : Problem(status, subtype, title, detail, data)
{
    public class InvalidFormat() : AuthenticationProblem(
        401,
        "invalid-format",
        "invalid format",
        "Request's token is in an invalid format, not in UUID format."
    );

    public class InvalidToken() : AuthenticationProblem(
        401,
        "invalid-token",
        "invalid token",
        "Request's token is missing"
    );

    public class UnauthorizedResource() : AuthenticationProblem(
        403,
        "unauthorized-resource",
        "unauthorized resource",
        "No permission to access this resource"
    );
}