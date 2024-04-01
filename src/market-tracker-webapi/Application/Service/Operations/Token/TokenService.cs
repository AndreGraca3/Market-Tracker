using market_tracker_webapi.Application.Http.Models.Token;
using market_tracker_webapi.Application.Repository.Operations.Token;
using market_tracker_webapi.Application.Repository.Operations.User;
using market_tracker_webapi.Application.Service.Errors.Token;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Token;

public class TokenService(
    IUserRepository userRepository,
    ITokenRepository tokenRepository,
    TransactionManager transactionManager
) : ITokenService
{
    public async Task<Either<TokenCreationError, TokenOutputModel>> CreateTokenAsync(string email, string password)
    {
        if (email == "" || password == "")
        {
            return EitherExtensions.Failure<TokenCreationError, TokenOutputModel>(
                new TokenCreationError.InvalidCredentials()
            );
        }

        return await transactionManager.ExecuteAsync(async () =>
            {
                var user = await userRepository.GetUserByEmailAsync(email);
                if (user is null)
                {
                    return EitherExtensions.Failure<TokenCreationError, TokenOutputModel>(
                        new TokenCreationError.InvalidCredentials()
                    );
                }

                if (user.Password != password)
                {
                    return EitherExtensions.Failure<TokenCreationError, TokenOutputModel>(
                        new TokenCreationError.InvalidCredentials()
                    );
                }

                var token = await tokenRepository.GetTokenByUserIdAsync(user.Id);
                if (token is not null) // if token exists
                {
                    if (token.ExpiresAt >= DateTime.Now) // if it has expired 
                    {
                        await tokenRepository.DeleteTokenAsync(token.TokenValue); // delete it
                    }

                    return EitherExtensions.Success<TokenCreationError, TokenOutputModel>(
                        new TokenOutputModel(token.TokenValue)
                    );
                }

                // no token found, create new one
                var newToken = await tokenRepository.CreateTokenAsync(user.Id);
                return EitherExtensions.Success<TokenCreationError, TokenOutputModel>(
                    new TokenOutputModel(newToken)
                );
            }
        );
    }

    public async Task<Either<TokenFetchingError, TokenOutputModel>> DeleteTokenAsync(Guid tokenValue)
    {
        return await transactionManager.ExecuteAsync(async () =>
            await tokenRepository.DeleteTokenAsync(tokenValue) is null
                ? EitherExtensions.Failure<TokenFetchingError, TokenOutputModel>(
                    new TokenFetchingError.TokenByTokenValueNotFound(tokenValue)
                )
                : EitherExtensions.Success<TokenFetchingError, TokenOutputModel>(new TokenOutputModel(tokenValue)));
    }
}