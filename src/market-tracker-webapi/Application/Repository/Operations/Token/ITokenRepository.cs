namespace market_tracker_webapi.Application.Repository.Operations.Token;

using Token = Domain.Token;

public interface ITokenRepository
{
    Task<Token?> GetTokenByTokenValueAsync(Guid tokenValue);

    Task<Token> CreateTokenAsync(Guid userId);

    Task<Guid> CreateTokenByTokenValueAsync(Guid tokenValue, DateTime expiresAt, Guid userId);

    Task<Token?> GetTokenByUserIdAsync(Guid userId);
    
    Task<Token?> DeleteTokenAsync(Guid tokenValue);
}