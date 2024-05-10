using market_tracker_webapi.Application.Repository.Account.Credential;
using market_tracker_webapi.Application.Repository.Account.Token;
using market_tracker_webapi.Application.Repository.Account.Users.User;
using market_tracker_webapi.Application.Service.Errors.Token;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;
using Microsoft.IdentityModel.Tokens;

namespace market_tracker_webapi.Application.Service.Operations.Account.Auth.Token;

using Token = Domain.Models.Account.Auth.Token;

public class TokenService(
    IUserRepository userRepository,
    ITokenRepository tokenRepository,
    IAccountRepository accountRepository,
    ITransactionManager transactionManager
) : ITokenService
{
    public async Task<Either<TokenCreationError, Token>> CreateTokenAsync(
        string email,
        string password
    )
    {
        if (email.IsNullOrEmpty() || password.IsNullOrEmpty())
        {
            return EitherExtensions.Failure<TokenCreationError, Token>(
                new TokenCreationError.InvalidCredentials()
            );
        }

        return await transactionManager.ExecuteAsync(async () =>
        {
            var user = await userRepository.GetUserByEmailAsync(email);
            if (user is null)
            {
                return EitherExtensions.Failure<TokenCreationError, Token>(
                    new TokenCreationError.InvalidCredentials()
                );
            }

            if ((await accountRepository.GetPasswordByUserIdAsync(user.Id))?.Password != password)
            {
                return EitherExtensions.Failure<TokenCreationError, Token>(
                    new TokenCreationError.InvalidCredentials()
                );
            }

            var token = await tokenRepository.GetTokenByUserIdAsync(user.Id);
            if (token is not null) // if token exists
            {
                if (token.ExpiresAt <= DateTime.Now) // if it has expired
                {
                    await tokenRepository.DeleteTokenAsync(token.TokenValue); // delete it
                    return EitherExtensions.Success<TokenCreationError, Token>(
                        await tokenRepository.CreateTokenAsync(user.Id)
                    );
                }

                return EitherExtensions.Success<TokenCreationError, Token>(
                    token
                );
            }

            // no token found, create new one
            var newToken = await tokenRepository.CreateTokenAsync(user.Id);
            return EitherExtensions.Success<TokenCreationError, Token>(
                newToken
            );
        });
    }

    public async Task<Either<TokenFetchingError, Token>> DeleteTokenAsync(
        string tokenValue
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
            {
                var deletedToken = await tokenRepository.DeleteTokenAsync(new Guid(tokenValue));
                if (deletedToken is null)
                {
                    return EitherExtensions.Failure<TokenFetchingError, Token>(
                        new TokenFetchingError.TokenByTokenValueNotFound(tokenValue)
                    );
                }

                return EitherExtensions.Success<TokenFetchingError, Token>(
                    deletedToken
                );
            }
        );
    }
}