namespace market_tracker_webapi.Application.Service.Errors.Token;

public class TokenFetchingError : ITokenError
{
    public class TokenByTokenValueNotFound(string tokenValue) : TokenFetchingError
    {
        public string TokenValue { get; } = tokenValue;
    }
}