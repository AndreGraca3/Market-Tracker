using market_tracker_webapi.Application.Service.Errors.Token;

namespace market_tracker_webapi.Application.Http.Problem;

public class TokenProblem(
    int status,
    string subType,
    string title,
    string detail,
    object? data = null
) : Problem(status, subType, title, detail, data)
{
    public class InvalidCredentials() : UserProblem(
        401,
        "invalid-credentials",
        "Invalid credentials",
        "The email and password provided are invalid"
    );

    public class TokenByTokenValueNotFound(TokenFetchingError.TokenByTokenValueNotFound data) : UserProblem(
        404,
        "token-not-found",
        "token not found",
        $"Token with value {data.TokenValue} not found",
        data
    );
}