﻿using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Service.Operations.Account.Users.User;

namespace market_tracker_webapi.Application.Http.Pipeline.Authorization;

public class RequestTokenProcessor(IUserService userService)
{
    public static string Scheme = "Bearer";

    public async Task<AuthenticatedUser?> ProcessAuthorizationHeaderValue(Guid authorizationValue)
    {
        return await userService.GetUserByToken(authorizationValue);
    }
}