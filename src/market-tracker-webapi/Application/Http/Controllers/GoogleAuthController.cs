﻿using market_tracker_webapi.Application.Http.Models.GoogleToken;
using market_tracker_webapi.Application.Http.Models.Token;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Service.Errors.Google;
using market_tracker_webapi.Application.Service.Operations.GoogleAuth;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers;

[ApiController]
[Route(Uris.Google.Base)]
public class GoogleAuthController(IGoogleAuthService googleAuthService) : Controller
{
    [HttpPost]
    public async Task<ActionResult<TokenOutputModel>> CreateGoogleAccountAsync(
        [FromBody] GoogleTokenCreationInputModel googleJsonWebToken
    )
    {
        return ResultHandler.Handle(
            await googleAuthService.CreateTokenAsync(googleJsonWebToken.IdToken),
            error =>
            {
                return error switch
                {
                    GoogleTokenCreationError.InvalidIssuer invalidIssuer
                        => new GoogleProblem.InvalidIssuer(invalidIssuer).ToActionResult(),

                    GoogleTokenCreationError.InvalidValue =>
                        new GoogleProblem.InvalidTokenFormat().ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            },
            tokenOutputModel => new OkObjectResult(tokenOutputModel));
    }
}