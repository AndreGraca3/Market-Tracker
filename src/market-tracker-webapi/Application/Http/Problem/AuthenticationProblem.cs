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
        "Invalid format",
        "Request's token is in an invalid format, not in UUID format."
    );

    public class InvalidToken() : AuthenticationProblem(
        401,
        "invalid-token",
        "Invalid token",
        "Request's token is invalid or expired"
    );

    public class AccessDenied() : AuthenticationProblem(
        403,
        "access-denied",
        "Access denied",
        "You do not have the necessary permissions to perform this action"
    );
}