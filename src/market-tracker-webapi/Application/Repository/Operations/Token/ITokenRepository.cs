namespace market_tracker_webapi.Application.Repository.Operations.Token;

using Token = Domain.Token;

public interface ITokenRepository
{
    Task<Token?> GetTokenByTokenValueAsync(Guid tokenValue);

    Task<Guid> CreateTokenAsync(Guid userId);

    Task<Token?> GetTokenByUserIdAsync(Guid userId);
    
    Task<Token?> DeleteTokenAsync(Guid tokenValue);
}