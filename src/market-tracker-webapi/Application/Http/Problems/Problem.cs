using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Problems;

public abstract class Problem(
    int status,
    string subtype,
    string title,
    string detail,
    object? data = null
)
{
    public const string MediaType = "application/problem+json";

    public Guid Id { get; } = Guid.NewGuid();
    public string Type { get; } = "https://markettracker.pt/probs/" + subtype;
    public string Title { get; } = title;
    public int Status { get; } = status;
    public string Detail { get; } = detail;
    public DateTime Timestamp { get; } = DateTime.Now;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Data { get; } = data;

    public override String ToString() => JsonSerializer.Serialize(this);
    
    public ActionResult ToActionResult()
    {
        return new ObjectResult(this) { StatusCode = Status, ContentTypes = { MediaType } };
    }
}
