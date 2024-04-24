namespace market_tracker_webapi.Application.Pipeline.Authorization;

public class AuthenticatedAttribute : Attribute
{
    public string? Role;

    private static readonly string[] ValidRoles = ["moderator", "operator", "client"];

    public AuthenticatedAttribute(string role)
    {
        if (Role is not null && !ValidRoles.Contains(Role))
        {
            throw new ArgumentException($"Invalid role parameter: {role}");
        }
        Role = role;
    }
}
