using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Problem;

public abstract class Problem(
    int status,
    string subtype,
    string title,
    string detail,
    object? data = null
)
{
    public const string MediaType = "application/problem+json";

    public string Type { get; } = "https://gomokuroyale.pt/probs/" + subtype;
    public string Title { get; } = title;
    public int Status { get; } = status;
    public string Detail { get; } = detail;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Data { get; } = data;

    public ActionResult ToActionResult()
    {
        return new ObjectResult(this) { StatusCode = Status, ContentTypes = { MediaType } };
    }
}
