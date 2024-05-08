namespace market_tracker_webapi.Application.Domain.Models.Account.Users;

public record Operator(
    Guid Id,
    string Name,
    string Email,
    int PhoneNumber,
    DateTime CreatedAt
)
{
    public Operator(User user, int phoneNumber) : this(
        user.Id,
        user.Name,
        user.Email,
        phoneNumber,
        user.CreatedAt
    )
    {
    }
};