using market_tracker_webapi.Application.Http.Problem;
using Microsoft.AspNetCore.Diagnostics;

namespace market_tracker_webapi.Application.Http.Pipeline;

public class CustomExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(
            new ServerProblem.InternalServerError(exception.Message)
        );

        return true;
    }
}
