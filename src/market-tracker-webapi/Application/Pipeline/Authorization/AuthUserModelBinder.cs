using market_tracker_webapi.Application.Service.Operations.User;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace market_tracker_webapi.Application.Pipeline.Authorization;

public class AuthUserModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        // Fetch the authenticated user
        var userService = bindingContext.HttpContext.RequestServices.GetService<IUserService>();
        var token = bindingContext.HttpContext.Request.Headers.Authorization;
        var authenticatedUser = await userService.GetUserByToken(new Guid(token!));

        // Set the model binding result
        bindingContext.Result = ModelBindingResult.Success(authenticatedUser);
    }
}
