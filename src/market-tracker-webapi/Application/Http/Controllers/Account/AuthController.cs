using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Http.Models.Schemas.Account.Auth;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Service.Operations.Account.Auth.GoogleAuth;
using market_tracker_webapi.Application.Service.Operations.Account.Auth.Token;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Account;

[ApiController]
[Produces(Uris.JsonMediaType, Uris.JsonProblemMediaType)]
public class AuthController(IGoogleAuthService googleAuthService, ITokenService tokenService) : ControllerBase
{
    [HttpPost(Uris.Auth.GoogleAuth)]
    public async Task<ActionResult> CreateGoogleTokenAsync(
        [FromBody] GoogleTokenCreationInputModel googleJsonWebToken)
    {
        var token = await googleAuthService.CreateTokenAsync(googleJsonWebToken.IdToken);

        HttpContext.Response.Cookies.Append(AuthenticationDetails.NameAuthorizationCookie,
            token.Value.ToString(), new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = token.ExpiresAt
            });

        return NoContent();
    }

    [HttpPost(Uris.Auth.Login)]
    public async Task<ActionResult<Token>> CreateTokenAsync(
        [FromBody] TokenCreationInputModel userCredentials
    )
    {
        var token = await tokenService.CreateTokenAsync(
            userCredentials.Email,
            userCredentials.Password
        );

        HttpContext.Response.Cookies.Append(AuthenticationDetails.NameAuthorizationCookie,
            token.Value.ToString(), new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.SpecifyKind(token.ExpiresAt, DateTimeKind.Utc)
            });
        return new NoContentResult();
    }

    [HttpDelete(Uris.Auth.Logout)]
    [Authorized([Role.Client, Role.Operator, Role.Moderator])]
    public async Task<ActionResult<Token>> RevokeTokenAsync()
    {
        var tokenToDelete = HttpContext.Request.Cookies[AuthenticationDetails.NameAuthorizationCookie]!;
        await tokenService.DeleteTokenAsync(tokenToDelete);

        HttpContext.Response.Cookies.Append(AuthenticationDetails.NameAuthorizationCookie, string.Empty,
            new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now
            });
        return new NoContentResult();
    }
}