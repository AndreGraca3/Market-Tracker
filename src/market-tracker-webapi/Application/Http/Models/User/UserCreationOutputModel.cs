namespace market_tracker_webapi.Application.Http.Models.User;

public record UserCreationOutputModel(Guid Id, string Role)
{
    public const string Client = "Client";
    public const string Operator = "Operator";
    public const string Moderator = "Moderator";
};