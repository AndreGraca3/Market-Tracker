using Microsoft.IdentityModel.Tokens;

namespace market_tracker_webapi.Application.Pipeline.Authorization;

[AttributeUsage(AttributeTargets.Method)]
public class AuthorizedAttribute(Role[] roles) : Attribute
{
    public readonly Role[] Roles = roles;
}

public enum Role
{
    Moderator,
    Operator,
    Client
}

static class RoleExtensions
{
    public static Role ToRole(this string role) => role switch
    {
        "Moderator" => Role.Moderator,
        "Operator" => Role.Operator,
        "Client" => Role.Client,
        _ => throw new ArgumentException("Invalid role")
    };

    public static bool ContainsRole(this Role[] roles, string role) => roles.Contains(role.ToRole());
}