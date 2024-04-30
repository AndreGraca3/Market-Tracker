using market_tracker_webapi.Application.Http.Models.GoogleToken;
using market_tracker_webapi.Application.Http.Models.Token;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Pipeline.Authorization;
using market_tracker_webapi.Application.Service.Errors.Google;
using market_tracker_webapi.Application.Service.Errors.Token;
using market_tracker_webapi.Application.Service.Operations.GoogleAuth;
using market_tracker_webapi.Application.Service.Operations.Token;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers
{
    [ApiController]
    [Route(Uris.Auth.Base)]
    public class AuthController(
        IGoogleAuthService googleAuthService,
        ITokenService tokenService,
        ILogger<AuthController> logger
    )
        : ControllerBase
    {
        [HttpPost(Uris.Auth.GoogleAuth)]
        public async Task<ActionResult<TokenOutputModel>> CreateGoogleTokenAsync(
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
                            new GoogleProblem.InvalidTokenFormat().ToActionResult()
                    };
                },
                tokenOutputModel =>
                {
                    HttpContext.Response.Cookies.Append(AuthenticationDetails.NameAuthorizationCookie,
                        tokenOutputModel.TokenValue.ToString(), new CookieOptions
                        {
                            HttpOnly = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = tokenOutputModel.ExpiresAt
                        });
                    return new NoContentResult();
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult<TokenOutputModel>> CreateTokenAsync(
            [FromBody] TokenCreationInputModel userCredentials
        )
        {
            logger.LogDebug(
                $"Call {nameof(CreateTokenAsync)} with {userCredentials.Email} and {userCredentials.Password}"
            );

            return ResultHandler.Handle(
                await tokenService.CreateTokenAsync(
                    userCredentials.Email,
                    userCredentials.Password
                ),
                error =>
                {
                    return error switch
                    {
                        TokenCreationError.InvalidCredentials
                            => new TokenProblem.InvalidCredentials().ToActionResult()
                    };
                },
                tokenOutputModel =>
                {
                    HttpContext.Response.Cookies.Append(AuthenticationDetails.NameAuthorizationCookie,
                        tokenOutputModel.TokenValue.ToString(), new CookieOptions
                        {
                            HttpOnly = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = tokenOutputModel.ExpiresAt
                        });
                    return new NoContentResult();
                });
        }

        [HttpDelete]
        public async Task<ActionResult<TokenOutputModel>> DeleteTokenAsync(
            [FromHeader] string tokenValue
        )
        {
            logger.LogDebug($"Call {nameof(DeleteTokenAsync)} with {tokenValue}");

            return ResultHandler.Handle(
                await tokenService.DeleteTokenAsync(tokenValue),
                error =>
                {
                    return error switch
                    {
                        TokenFetchingError.TokenByTokenValueNotFound tokenByTokenValueNotFound
                            => new TokenProblem.TokenByTokenValueNotFound(
                                tokenByTokenValueNotFound
                            ).ToActionResult()
                    };
                },
                _ =>
                {
                    HttpContext.Response.Cookies.Delete(AuthenticationDetails.NameAuthorizationCookie);
                    return new NoContentResult();
                }
            );
        }
    }
}