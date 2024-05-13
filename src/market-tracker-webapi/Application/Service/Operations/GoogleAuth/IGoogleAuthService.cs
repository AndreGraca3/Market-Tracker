﻿using market_tracker_webapi.Application.Http.Models.Token;
using market_tracker_webapi.Application.Service.Errors.Google;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.GoogleAuth;

public interface IGoogleAuthService
{
    Task<Either<GoogleTokenCreationError, TokenOutputModel>> CreateTokenAsync(
        string tokenValue
    );
}