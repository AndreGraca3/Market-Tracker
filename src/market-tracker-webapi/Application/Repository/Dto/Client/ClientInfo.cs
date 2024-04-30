namespace market_tracker_webapi.Application.Repository.Dto.Client;

public record ClientInfo(
    Guid Id,
    string Username,
    string Name,
    DateTime CreatedAt,
    string? AvatarUrl
)
{
    public ClientInfo(Domain.User user, string? avatar) : this(
        user.Id,
        user.Username,
        user.Name,
        user.CreatedAt,
        avatar
    )
    {
    }
};