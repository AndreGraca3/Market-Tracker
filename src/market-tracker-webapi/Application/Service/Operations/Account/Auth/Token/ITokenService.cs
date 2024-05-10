using market_tracker_webapi.Application.Service.Errors.Token;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Account.Auth.Token;

using Token = Domain.Models.Account.Auth.Token;

public interface ITokenService
{
    Task<Either<TokenCreationError, Token>> CreateTokenAsync(
        string email,
        string password
    );

    Task<Either<TokenFetchingError, Token>> DeleteTokenAsync(
        string tokenValue
    );
}