using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Service.Operations.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace market_tracker_webapi.Application.Pipeline.authorization;

public class AuthenticationFilter(ICategoryService categoryService) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (!context.ActionDescriptor.EndpointMetadata.Any(e => e is AuthenticatedAttribute))
        {
            return;
        }

        var token = context.HttpContext.Request.Headers.Authorization.ToString();
        if (string.IsNullOrEmpty(token))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var c = await categoryService.GetCategoriesAsync();
        Console.WriteLine("Categories");

        context.HttpContext.Items["User"] = new AuthenticatedUser(token);
    }
}