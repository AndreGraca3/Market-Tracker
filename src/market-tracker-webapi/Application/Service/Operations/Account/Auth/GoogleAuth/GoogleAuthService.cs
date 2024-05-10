using Google.Apis.Auth;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Repository.Account.Token;
using market_tracker_webapi.Application.Repository.Account.Users.Client;
using market_tracker_webapi.Application.Repository.Account.Users.User;
using market_tracker_webapi.Application.Service.Errors.Google;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;
using Microsoft.IdentityModel.Tokens;

namespace market_tracker_webapi.Application.Service.Operations.Account.Auth.GoogleAuth;

using Token = Domain.Models.Account.Auth.Token;

public class GoogleAuthService(
    IClientRepository clientRepository,
    IUserRepository userRepository,
    ITokenRepository tokenRepository,
    ITransactionManager transactionManager
) : IGoogleAuthService
{
    private const string ServerId = "317635904868-mgmlu2g27gt43tb00c7i5kfevprerrsn.apps.googleusercontent.com";

    public async Task<Either<GoogleTokenCreationError, Token>> CreateTokenAsync(string tokenValue)
    {
        if (tokenValue.IsNullOrEmpty())
        {
            return EitherExtensions.Failure<GoogleTokenCreationError, Token>(
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

                var userId = (await userRepository.GetUserByEmailAsync(payload.Email))?.Id.Value ??
                             (await userRepository
                                 .CreateUserAsync(
                                     payload.Name,
                                     payload.Email,
                                     Role.Client.ToString()
                                 )).Value;

                await clientRepository.CreateClientAsync(userId, payload.Email.Split("@").First(), payload.Picture);

                return EitherExtensions.Success<GoogleTokenCreationError, Token>(
                    await tokenRepository.CreateTokenAsync(userId)
                );
            }
            catch (InvalidJwtException)
            {
                return EitherExtensions.Failure<GoogleTokenCreationError, Token>(
                    new GoogleTokenCreationError.InvalidIssuer("google")
                );
            }
        });
    }
}