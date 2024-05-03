using market_tracker_webapi.Application.Http.Problem;
using Microsoft.AspNetCore.Mvc.Filters;

namespace market_tracker_webapi.Application.Pipeline.authorization;

public class AuthorizationFilter(RequestTokenProcessor tokenProcessor) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            if (context.ActionDescriptor.EndpointMetadata.FirstOrDefault(e =>
                    e is AuthorizedAttribute) is not AuthorizedAttribute authorizedAttribute) return;

            var tokenValue =
                context.HttpContext.Request.Headers[AuthenticationDetails.NameAuthorizationHeader].ToString();

            if (string.IsNullOrWhiteSpace(tokenValue))
            {
                context.HttpContext.Response.Headers.Append(AuthenticationDetails.NameWwwAuthenticateHeader,
                    RequestTokenProcessor.Scheme);
                context.Result = new AuthenticationProblem.InvalidToken().ToActionResult();
                return;
            }

            var authenticatedUser = await tokenProcessor.ProcessAuthorizationHeaderValue(Guid.Parse(tokenValue));

            if (authenticatedUser is null)
            {
                context.Result = new AuthenticationProblem.InvalidToken().ToActionResult();
                return;
            }

            if (!authorizedAttribute.Roles.ContainsRole(authenticatedUser.User.Role))
            {
                context.Result = new AuthenticationProblem.AccessDenied().ToActionResult();
                return;
            }

            context.HttpContext.Items[AuthenticationDetails.KeyUser] = authenticatedUser;
        }
        catch (Exception e)
        {
            context.Result = e switch
            {
                ArgumentNullException => new AuthenticationProblem.InvalidToken().ToActionResult(),
                FormatException => new AuthenticationProblem.InvalidFormat().ToActionResult(),
                _ => new ServerProblem.InternalServerError().ToActionResult()
            };
        }
    }
}