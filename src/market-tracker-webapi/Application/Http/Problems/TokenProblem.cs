using market_tracker_webapi.Application.Service.Errors.Token;

namespace market_tracker_webapi.Application.Http.Problems;

public class TokenProblem(
    int status,
    string subType,
    string title,
    string detail,
    object? data = null
) : Problem(status, subType, title, detail, data)
{
    public class InvalidCredentials() : TokenProblem(
        401,
        "invalid-credentials",
        "Invalid credentials",
        "The email and password provided are invalid"
    );

    public class TokenByTokenValueNotFound(TokenFetchingError.TokenByTokenValueNotFound data) : TokenProblem(
        404,
        "token-not-found",
        "token not found",
        $"Token with value {data.TokenValue} not found",
        data
    );
    
    public static TokenProblem FromServiceError(ITokenError error)
        => error switch
        {
            TokenFetchingError.TokenByTokenValueNotFound tokenByTokenValueNotFound => new TokenByTokenValueNotFound(tokenByTokenValueNotFound),
            TokenCreationError.InvalidCredentials invalidCredentials => new InvalidCredentials(),
            _ => throw new ArgumentOutOfRangeException(nameof(error))
        };
}