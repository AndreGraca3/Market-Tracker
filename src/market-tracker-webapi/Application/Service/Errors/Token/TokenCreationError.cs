namespace market_tracker_webapi.Application.Service.Errors.Token;

public class TokenCreationError : TokenError
{
    public class InvalidCredentials() : TokenCreationError;
}