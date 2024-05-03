using Google.Apis.Auth;
using market_tracker_webapi.Application.Http.Models.Token;
using market_tracker_webapi.Application.Repository.Operations.Client;
using market_tracker_webapi.Application.Repository.Operations.Token;
using market_tracker_webapi.Application.Repository.Operations.User;
using market_tracker_webapi.Application.Service.Errors.Google;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;
using Microsoft.IdentityModel.Tokens;

namespace market_tracker_webapi.Application.Service.Operations.GoogleAuth;

public class GoogleAuthService(
    IClientRepository clientRepository,
    IUserRepository userRepository,
    ITokenRepository tokenRepository,
    ITransactionManager transactionManager
) : IGoogleAuthService
{
    private const string ServerId = "317635904868-mgmlu2g27gt43tb00c7i5kfevprerrsn.apps.googleusercontent.com";

    public async Task<Either<GoogleTokenCreationError, TokenOutputModel>> CreateTokenAsync(string tokenValue)
    {
        if (tokenValue.IsNullOrEmpty())
        {
            return EitherExtensions.Failure<GoogleTokenCreationError, TokenOutputModel>(
                new GoogleTokenCreationError.InvalidValue()
            );
        }

        return await transactionManager.ExecuteAsync(async () =>
        {
            try
            {
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { ServerId }
                };

                GoogleJsonWebSignature.Payload payload =
                    await GoogleJsonWebSignature.ValidateAsync(tokenValue, validationSettings);

                var userId = (await userRepository.GetUserByEmailAsync(payload.Email))?.Id ?? await userRepository
                    .CreateUserAsync(
                        payload.Name,
                        payload.Email,
                        "Client"
                    );

                await clientRepository.CreateClientAsync(userId, payload.Email.Split("@").First(), payload.Picture);

                var token = await tokenRepository.CreateTokenAsync(userId);

                return EitherExtensions.Success<GoogleTokenCreationError, TokenOutputModel>(
                    new TokenOutputModel(
                        token.TokenValue,
                        token.ExpiresAt
                    )
                );
            }
            catch (InvalidJwtException)
            {
                return EitherExtensions.Failure<GoogleTokenCreationError, TokenOutputModel>(
                    new GoogleTokenCreationError.InvalidIssuer("google")
                );
            }
        });
    }
}