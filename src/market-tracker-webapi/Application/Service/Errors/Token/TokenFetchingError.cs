namespace market_tracker_webapi.Application.Service.Errors.Token;

public class TokenFetchingError : TokenError
{
    public class TokenByTokenValueNotFound(String tokenValue) : TokenFetchingError
    {
        public String TokenValue { get; } = tokenValue;
    }
}