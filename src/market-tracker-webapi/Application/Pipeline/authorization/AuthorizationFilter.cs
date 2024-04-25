using market_tracker_webapi.Application.Http.Problem;
using Microsoft.AspNetCore.Mvc.Filters;

namespace market_tracker_webapi.Application.Pipeline.Authorization;

public class AuthorizationFilter(RequestTokenProcessor tokenProcessor) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var authorizedAttribute = context.ActionDescriptor.EndpointMetadata.FirstOrDefault(e =>
                e.GetType() == typeof(AuthorizedAttribute)) as AuthorizedAttribute;

            if (authorizedAttribute is null)
            {
                return;
            }

            var tokenValue = new Guid(
                context.HttpContext.Request.Cookies[AuthenticationDetails.NameAuthorizationCookie] ?? string.Empty
            );

            var authenticatedUser = await tokenProcessor.ProcessAuthorizationHeaderValue(tokenValue);

            if (authenticatedUser is null)
            {
                context.Result =
                    new AuthenticationProblem.InvalidToken().ToActionResult();
                return;
            }

            if (!authorizedAttribute.Roles.Contains(authenticatedUser.User.Role))
            {
                context.Result =
                    new AuthenticationProblem.UnauthorizedResource().ToActionResult();
                return;
            }

            Console.WriteLine($"Authenticated User in Filter: {authenticatedUser}");

            context.HttpContext.Items[AuthenticationDetails.KeyUser] = authenticatedUser;
        }
        catch (Exception e)
        {
            context.Result = e switch
            {
                ArgumentNullException => new AuthenticationProblem.InvalidToken().ToActionResult(),
                FormatException => new AuthenticationProblem.InvalidFormat().ToActionResult(),
            };
        }
    }
}