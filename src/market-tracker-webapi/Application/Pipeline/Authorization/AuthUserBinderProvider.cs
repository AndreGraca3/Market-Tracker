using market_tracker_webapi.Application.Http.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace market_tracker_webapi.Application.Pipeline.Authorization;

public class AuthUserBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Metadata.ModelType == typeof(AuthenticatedUser))
        {
            return new BinderTypeModelBinder(typeof(AuthUserModelBinder));
        }

        return null;
    }
}
