namespace market_tracker_webapi.Application.Service.Errors.Google;

public class GoogleTokenCreationError: IGoogleTokenError
{
    public class InvalidGoogleToken(string token) : GoogleTokenCreationError;
}