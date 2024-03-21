namespace market_tracker_webapi.Application.Http.Problem;

public class Problem(int status, string subtype, string title, string detail, object? data = null)
{
    public const string MediaType = "application/problem+json";

    public string Type { get; } = "https://gomokuroyale.pt/probs/" + status;
    public string Title { get; } = title;
    public int Status { get; } = status;
    public string Detail { get; } = detail;
    public object? Data { get; } = data;
}
