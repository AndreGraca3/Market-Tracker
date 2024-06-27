using market_tracker_webapi.Application.Repository.Account.Credential;
using market_tracker_webapi.Application.Repository.Account.Token;
using market_tracker_webapi.Application.Repository.Account.Users.User;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Token;
using market_tracker_webapi.Application.Service.Transaction;
using Microsoft.IdentityModel.Tokens;

namespace market_tracker_webapi.Application.Service.Operations.Account.Auth.Token;

using Token = Domain.Schemas.Account.Auth.Token;

public class TokenService(
    IUserRepository userRepository,
    ITokenRepository tokenRepository,
    IAccountRepository accountRepository,
    ITransactionManager transactionManager
) : ITokenService
{
    public async Task<Token> CreateTokenAsync(string email, string password)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (email.IsNullOrEmpty() || password.IsNullOrEmpty())
            {
                throw new MarketTrackerServiceException(
                    new TokenCreationError.InvalidCredentials()
                );
            }

            var user = await userRepository.GetUserByEmailAsync(email);
            if (user is null)
            {
                throw new MarketTrackerServiceException(
                    new TokenCreationError.InvalidCredentials()
                );
            }

            if ((await accountRepository.GetPasswordByUserIdAsync(user.Id.Value))?.Password != password)
            {
                throw new MarketTrackerServiceException(
                    new TokenCreationError.InvalidCredentials()
                );
            }

            var token = await tokenRepository.GetTokenByUserIdAsync(user.Id.Value);
            if (token is not null)
            {
                // still valid
                if (token.ExpiresAt > DateTime.UtcNow) return token;

                // expired, delete it
                await tokenRepository.DeleteTokenAsync(token.Value);
                return await tokenRepository.CreateTokenAsync(user.Id.Value);
            }

            // no token found, create new one
            var newToken = await tokenRepository.CreateTokenAsync(user.Id.Value);
            return newToken;
        });
    }

    public async Task<Token> DeleteTokenAsync(string tokenValue)
    {
        return await transactionManager.ExecuteAsync(async () =>
            await tokenRepository.DeleteTokenAsync(new Guid(tokenValue)) ?? throw new MarketTrackerServiceException(
                new TokenFetchingError.TokenByTokenValueNotFound(tokenValue)
            ));
    }
}