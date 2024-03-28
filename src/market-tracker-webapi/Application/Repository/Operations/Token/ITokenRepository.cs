using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Models;

namespace market_tracker_webapi.Application.Repositories.Token;

public interface ITokenRepository
{
    Task<TokenData> CreateTokenAsync(string tokenValue, int userId);

    Task<AuthenticatedUserData?> GetUserAndTokenByTokenValueAsync(string token);

    Task<TokenData?> GetTokenByUserIdAsync(Guid userId);

    Task UpdateTokenLastUsedAsync(TokenData tokenData, DateTime now);

    Task DeleteTokenAsync(string token);
}