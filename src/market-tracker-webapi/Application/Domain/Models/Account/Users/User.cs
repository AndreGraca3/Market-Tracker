namespace market_tracker_webapi.Application.Domain.Models.Account.Users;

public record User(
    UserId Id,
    string Name,
    string Email,
    string Role,
    DateTime CreatedAt
)
{
    public User(
        Guid Id,
        string Name,
        string Email,
        string Role,
        DateTime CreatedAt
    ) : this(
        new UserId(Id),
        Name,
        Email,
        Role,
        CreatedAt
    )
    {
    }
};

public record UserItem(
    Guid Id,
    string Name,
    string Role
);

public record UserId(
    Guid Value
);