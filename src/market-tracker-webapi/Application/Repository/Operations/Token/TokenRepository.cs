using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.Token;

using Token = Domain.Token;

public class TokenRepository(
    MarketTrackerDataContext dataContext
) : ITokenRepository
{
    public async Task<Token?> GetTokenByTokenValueAsync(Guid tokenValue)
    {
        return MapTokenEntity(await dataContext.Token.FindAsync(tokenValue));
    }

    public async Task<Guid> CreateTokenAsync(Guid userId)
    {
        var newToken = new TokenEntity
        {
            CreatedAt = default,
            ExpiresAt = default,
            UserId = userId
        };
        await dataContext.Token.AddAsync(newToken);
        await dataContext.SaveChangesAsync();
        return newToken.TokenValue;
    }

    public async Task<Token?> GetTokenByUserIdAsync(Guid userId)
    {
        return MapTokenEntity(await dataContext.Token.Where(token => token.UserId == userId).FirstOrDefaultAsync());
    }

    public async Task<Token?> DeleteTokenAsync(Guid tokenValue)
    {
        var deletedToken = await dataContext.Token.FindAsync(tokenValue);
        if (deletedToken is null)
        {
            return null;
        }

        dataContext.Remove(deletedToken);
        await dataContext.SaveChangesAsync();
        return MapTokenEntity(deletedToken);
    }

    private static Token? MapTokenEntity(TokenEntity? tokenEntity)
    {
        return tokenEntity is not null
            ? new Token
            (
                tokenEntity.TokenValue,
                tokenEntity.CreatedAt,
                tokenEntity.ExpiresAt,
                tokenEntity.UserId
            )
            : null;
    }
}