using market_tracker_webapi.Application.Service.Errors.Google;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Account.Auth.GoogleAuth;

using Token = Domain.Models.Account.Auth.Token;

public interface IGoogleAuthService
{
    Task<Either<GoogleTokenCreationError, Token>> CreateTokenAsync(
        string tokenValue
    );
}