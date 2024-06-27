using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Account.Auth;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Account.Token;

using Token = Domain.Schemas.Account.Auth.Token;

public class TokenRepository(
    MarketTrackerDataContext dataContext
) : ITokenRepository
{
    public async Task<Token?> GetTokenByTokenValueAsync(Guid tokenValue)
    {
        return (await dataContext.Token.FindAsync(tokenValue))?.ToToken();
    }

    public async Task<Token> CreateTokenAsync(Guid userId)
    {
        var tokenEntity = new TokenEntity
        {
            ExpiresAt = DateTime.UtcNow.AddDays(1),
            UserId = userId
        };
        await dataContext.Token.AddAsync(tokenEntity);
        await dataContext.SaveChangesAsync();
        return tokenEntity.ToToken();
    }

    public async Task<Guid> CreateTokenByTokenValueAsync(Guid tokenValue, DateTime expiresAt, Guid userId)
    {
        var tokenEntity = new TokenEntity
        {
            TokenValue = tokenValue,
            ExpiresAt = expiresAt,
            UserId = userId
        };
        await dataContext.Token.AddAsync(tokenEntity);
        await dataContext.SaveChangesAsync();
        return tokenEntity.TokenValue;
    }

    public async Task<Token?> GetTokenByUserIdAsync(Guid userId)
    {
        return (await dataContext.Token.Where(tokenEntity => tokenEntity.UserId == userId).FirstOrDefaultAsync())?.ToToken();
    }

    public async Task<Token?> DeleteTokenAsync(Guid tokenValue)
    {
        var deletedTokenEntity = await dataContext.Token.FindAsync(tokenValue);
        if (deletedTokenEntity is null)
        {
            return null;
        }

        dataContext.Remove(deletedTokenEntity);
        await dataContext.SaveChangesAsync();
        return deletedTokenEntity.ToToken();
    }
}