using Microsoft.IdentityModel.Tokens;

namespace market_tracker_webapi.Application.Pipeline.Authorization;

public class AuthorizedAttribute : Attribute
{
    public string[] Roles;

    private static readonly string[] ValidRoles = ["moderator", "operator", "client"];

    public AuthorizedAttribute(string[] roles)
    {
        var invalidRole = Roles?.FirstOrDefault(it => !ValidRoles.Contains(it));
        if (!Roles.IsNullOrEmpty() && invalidRole != null)
        {
            throw new ArgumentException($"Invalid role parameter: {invalidRole}");
        }

        Roles = roles;
    }
}