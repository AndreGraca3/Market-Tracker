using market_tracker_webapi.Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http;

public static class ResultHandler
{
    public static ActionResult<T> Handle<TError, T>(
        Either<TError, T> result,
        Func<TError, ActionResult<T>> onError
    )
    {
        if (result.IsSuccessful())
        {
            return result.Value;
        }

        return onError(result.Error);
    }
}
