namespace market_tracker_webapi.Application.Repository.Account.Token;

using Token = Domain.Models.Account.Auth.Token;

public interface ITokenRepository
{
    Task<Token?> GetTokenByTokenValueAsync(Guid tokenValue);

    Task<Token> CreateTokenAsync(Guid userId);

    Task<Guid> CreateTokenByTokenValueAsync(Guid tokenValue, DateTime expiresAt, Guid userId);

    Task<Token?> GetTokenByUserIdAsync(Guid userId);
    
    Task<Token?> DeleteTokenAsync(Guid tokenValue);
}