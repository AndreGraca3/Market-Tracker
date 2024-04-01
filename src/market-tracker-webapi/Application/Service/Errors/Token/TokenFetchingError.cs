namespace market_tracker_webapi.Application.Service.Errors.Token;

public class TokenFetchingError : TokenError
{
    public class TokenByTokenValueNotFound(Guid tokenValue) : TokenFetchingError
    {
        public Guid TokenValue { get; } = tokenValue;
    }
}