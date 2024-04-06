namespace market_tracker_webapi.Application.Http.Problem;

public class AuthenticationProblem(
    int status,
    string subtype,
    string title,
    string detail,
    object? data = null
) : Problem(status, subtype, title, detail, data)
{
    public class InvalidToken() : AuthenticationProblem(
        401,
        "invalid-token",
        "invalid token",
        "Request's token is missing, invalid or expired"
    );
}