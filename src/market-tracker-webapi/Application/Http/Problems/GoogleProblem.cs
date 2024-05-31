using market_tracker_webapi.Application.Service.Errors.Google;

namespace market_tracker_webapi.Application.Http.Problems;

public class GoogleProblem(
    int status,
    string subType,
    string title,
    string detail,
    object? data = null
) : Problem(status, subType, title, detail, data)
{
    public class InvalidGoogleToken(GoogleTokenCreationError.InvalidGoogleToken data) : GoogleProblem(
        401,
        "invalid-google-token",
        "Invalid google token",
        "The token provided is not a Json Web Token",
        data
    );
    
    public static GoogleProblem FromServiceError(IGoogleTokenError error) => error switch
    {
        GoogleTokenCreationError.InvalidGoogleToken invalidGoogleToken => new InvalidGoogleToken(invalidGoogleToken),
        _ => throw new ArgumentOutOfRangeException(nameof(error))
    };
}