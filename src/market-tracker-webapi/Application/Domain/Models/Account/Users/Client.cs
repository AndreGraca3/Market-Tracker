namespace market_tracker_webapi.Application.Domain.Models.Account.Users;

public record Client(
    Guid Id,
    string? Username,
    string Name,
    string Email,
    DateTime CreatedAt,
    string? AvatarUrl
)
{
    public Client(User user, string? username, string? avatar) : this(
        user.Id,
        username,
        user.Name,
        user.Email,
        user.CreatedAt,
        avatar
    )
    {
    }
}

public record ClientItem(Guid Id, string Username, string? Avatar);