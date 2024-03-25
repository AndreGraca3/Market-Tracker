using market_tracker_webapi.Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http;

public static class ResultHandler
{
    /**
     * Handle the result of a service method.
     * If the result is successful, return an OkObjectResult with the value by default.
     */
    public static ActionResult<T> Handle<TError, T>(
        Either<TError, T> result,
        Func<TError, ActionResult<T>> onError,
        Func<T, ActionResult<T>>? onSuccess = null
    )
    {
        if (result.IsSuccessful())
        {
            return onSuccess?.Invoke(result.Value) ?? new OkObjectResult(result.Value);
        }

        return onError(result.Error);
    }
}
