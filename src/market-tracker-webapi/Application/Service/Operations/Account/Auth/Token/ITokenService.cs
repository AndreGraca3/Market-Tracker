namespace market_tracker_webapi.Application.Service.Operations.Account.Auth.Token;

using Token = Domain.Schemas.Account.Auth.Token;

public interface ITokenService
{
    Task<Token> CreateTokenAsync(string email, string password);

    Task<Token> DeleteTokenAsync(string tokenValue);
}