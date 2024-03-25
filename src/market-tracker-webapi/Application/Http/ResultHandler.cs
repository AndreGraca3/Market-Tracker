using market_tracker_webapi.Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http;

public static class ResultHandler
{
    public static ActionResult<T> Handle<TError, T>(
        Either<TError, T> result,
        Func<TError, ActionResult<T>> onError,
        Func<T, ActionResult<T>>? onSuccess = null
    )
    {
        if (result.IsSuccessful())
        {
            return onSuccess is null
                ? new OkObjectResult(result.Value)
                : onSuccess(result.Value);
        }

        return onError(result.Error);
    }
}
