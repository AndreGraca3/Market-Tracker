using market_tracker_webapi.Application.Http.Models.Token;
using market_tracker_webapi.Application.Service.Errors.Token;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Account.Auth.Token;

public interface ITokenService
{
    Task<Either<TokenCreationError, TokenOutputModel>> CreateTokenAsync(
        string email,
        string password
    );

    Task<Either<TokenFetchingError, TokenOutputModel>> DeleteTokenAsync(
        string tokenValue
    );
}