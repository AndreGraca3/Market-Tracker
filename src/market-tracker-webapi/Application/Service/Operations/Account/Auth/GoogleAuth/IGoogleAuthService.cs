namespace market_tracker_webapi.Application.Service.Operations.Account.Auth.GoogleAuth;

using Token = Domain.Schemas.Account.Auth.Token;

public interface IGoogleAuthService
{
    Task<Token> CreateTokenAsync(string tokenValue);
}