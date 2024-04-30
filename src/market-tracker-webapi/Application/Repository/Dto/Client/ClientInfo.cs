namespace market_tracker_webapi.Application.Repository.Dto.Client;

public record ClientInfo(
    Guid Id,
    string Username,
    string Name,
    string Email,
    DateTime CreatedAt,
    string? AvatarUrl
)
{
    public ClientInfo(Domain.User user, string? avatar) : this(
        user.Id,
        user.Username,
        user.Name,
        user.Email,
        user.CreatedAt,
        avatar
    )
    {
    }
};