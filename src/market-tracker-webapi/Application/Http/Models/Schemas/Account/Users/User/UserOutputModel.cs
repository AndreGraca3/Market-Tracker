namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.User;

using User = Domain.Schemas.Account.Users.User;

public record UserOutputModel(Guid Id, string Name, string Role, DateTime CreatedAt);

public static class UserModelMapper
{
    public static UserOutputModel ToOutputModel(this User user)
    {
        return new UserOutputModel(
            user.Id.Value,
            user.Name,
            user.Role,
            user.CreatedAt
        );
    }
}