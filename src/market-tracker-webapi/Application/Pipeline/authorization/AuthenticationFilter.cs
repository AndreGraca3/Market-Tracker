using market_tracker_webapi.Application.Http;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Operations.User;
using Microsoft.AspNetCore.Mvc.Filters;

namespace market_tracker_webapi.Application.Pipeline.Authorization;

public class AuthenticationFilter(IUserService userService) : IAsyncAuthorizationFilter
{
    private const string Schema = "Bearer ";
    public const string KeyUser = "User";

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (!context.ActionDescriptor.EndpointMetadata.Any(e => e is AuthenticatedAttribute))
        {
            return;
        }

        var token = context.HttpContext.Request.Headers.Authorization.ToString().Replace(Schema, "");
        if (string.IsNullOrEmpty(token))
        {
            context.Result =
                new AuthenticationProblem.InvalidToken().ToActionResult();
            Console.WriteLine("Unauthorized because token is null");
            return;
        }

        var user = await userService.GetUserByToken(new Guid(token));
        if (user is null)
        {
            context.Result =
                new AuthenticationProblem.InvalidToken().ToActionResult();
            Console.WriteLine("Unauthorized because token is null");
            return;
        }

        context.HttpContext.Items[KeyUser] = user;
    }
}