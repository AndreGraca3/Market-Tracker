namespace market_tracker_webapi.Application.Domain.Schemas.Account.Users;

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
    UserId Id,
    string Name,
    string Role
)
{
    public UserItem(Guid id, string name, string role) : this(
        new UserId(id),
        name,
        role
    )
    {
    }
}

public record UserId(Guid Value);