using market_tracker_webapi.Application.Http.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace market_tracker_webapi.Application.Pipeline.authorization;

public class AuthUserModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        // Fetch the authenticated user
        var token = bindingContext.HttpContext.Request.Headers.Authorization;
        var user = new AuthenticatedUser($"user test: {token}");

        // Set the model binding result
        bindingContext.Result = ModelBindingResult.Success(user);

        return Task.CompletedTask;
    }
}