namespace market_tracker_webapi.Application.Domain.Schemas.Account.Users;

public record Client(
    UserId Id,
    string Username,
    string Name,
    string Email,
    DateTime CreatedAt,
    string? AvatarUrl
)
{
    public Client(User user, string username, string? avatar) : this(
        user.Id,
        username,
        user.Name,
        user.Email,
        user.CreatedAt,
        avatar
    )
    {
    }
    
    public Client(Guid id, string username, string name, string email, DateTime createdAt, string? avatar) : this(
        new UserId(id),
        username,
        name,
        email,
        createdAt,
        avatar
    )
    {
    }
    
    public ClientItem ToClientItem() => new ClientItem(Id, Username, AvatarUrl);
}

public record ClientItem(UserId Id, string Username, string? Avatar)
{
    public ClientItem(Guid id, string username, string? avatar) : this(
        new UserId(id),
        username,
        avatar
    )
    {
    }
}