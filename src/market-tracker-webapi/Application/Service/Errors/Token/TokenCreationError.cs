namespace market_tracker_webapi.Application.Service.Errors.Token;

public class TokenCreationError : ITokenError
{
    public class InvalidCredentials : TokenCreationError;
}