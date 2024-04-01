namespace market_tracker_webapi.Application.Http.Problem;

public abstract class ServerProblem(
    int status,
    string subtype,
    string detail,
    object? data = null
) : Problem(status, subtype, "Internal Server Error", detail, data)
{
    public class InternalServerError(string detail, object? data = null)
        : ServerProblem(500, "internal-server-error", detail, data);
}
