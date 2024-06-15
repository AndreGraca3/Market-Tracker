using Google.Apis.Auth;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Repository.Account.Token;
using market_tracker_webapi.Application.Repository.Account.Users.Client;
using market_tracker_webapi.Application.Repository.Account.Users.User;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Google;
using market_tracker_webapi.Application.Service.Transaction;
using Microsoft.IdentityModel.Tokens;

namespace market_tracker_webapi.Application.Service.Operations.Account.Auth.GoogleAuth;

using Token = Domain.Schemas.Account.Auth.Token;

public class GoogleAuthService(
    IClientRepository clientRepository,
    IUserRepository userRepository,
    ITokenRepository tokenRepository,
    ITransactionManager transactionManager
) : IGoogleAuthService
{
    private const string ServerId = "317635904868-mgmlu2g27gt43tb00c7i5kfevprerrsn.apps.googleusercontent.com";

    public async Task<Token> CreateTokenAsync(string tokenValue)
    {
        if (tokenValue.IsNullOrEmpty())
            throw new MarketTrackerServiceException(new GoogleTokenCreationError.InvalidGoogleToken(tokenValue));

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

                var userId = (await userRepository.GetUserByEmailAsync(payload.Email))?.Id.Value;

                if (userId is null)
                {
                    userId = (await userRepository.CreateUserAsync(
                        payload.Name,
                        payload.Email,
                        Role.Client.ToString()
                    )).Value;

                    await clientRepository.CreateClientAsync(userId.Value, payload.Email.Split("@").First(),
                        payload.Picture);
                }

                return await tokenRepository.CreateTokenAsync(userId.Value);
            }
            catch (InvalidJwtException)
            {
                throw new MarketTrackerServiceException(new GoogleTokenCreationError.InvalidGoogleToken(tokenValue));
            }
        });
    }
}