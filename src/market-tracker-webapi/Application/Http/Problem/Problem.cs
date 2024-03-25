using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace market_tracker_webapi.Application.Http.Problem;

// [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class Problem(int status, string subtype, string title, string detail, object? data = null)
{
    public const string MediaType = "application/problem+json";

    public string Type { get; } = "https://gomokuroyale.pt/probs/" + subtype;
    public string Title { get; } = title;
    public int Status { get; } = status;
    public string Detail { get; } = detail;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public object? Data { get; } = data;

    public ActionResult ToActionResult()
    {
        return new ObjectResult(this) { StatusCode = Status, ContentTypes = { MediaType } };
    }
}
