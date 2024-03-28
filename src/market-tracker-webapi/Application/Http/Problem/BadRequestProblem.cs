namespace market_tracker_webapi.Application.Http.Problem;

public abstract class BadRequestProblem(
    int status,
    string subType,
    string detail,
    object? data = null
) : Problem(status, subType, "Invalid Request Content", detail, data)
{
    public class InvalidRequestContent(string detail)
        : BadRequestProblem(
            400,
            "invalid-request-content",
            detail
        );
}