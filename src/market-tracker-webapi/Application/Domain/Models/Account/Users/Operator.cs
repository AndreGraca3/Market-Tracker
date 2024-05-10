namespace market_tracker_webapi.Application.Domain.Models.Account.Users;

public record Operator(
    OperatorId Id,
    string Name,
    string Email,
    int PhoneNumber,
    DateTime CreatedAt
)
{
    public Operator(
        Guid Id,
        string Name,
        string Email,
        int PhoneNumber,
        DateTime CreatedAt
    ) : this(
        new OperatorId(Id),
        Name,
        Email,
        PhoneNumber,
        CreatedAt
    )
    {
    }


    public Operator(User user, int phoneNumber) : this(
        user.Id.Value,
        user.Name,
        user.Email,
        phoneNumber,
        user.CreatedAt
    )
    {
    }
};

public record OperatorItem(Guid Id, string Name, string StoreName);

public record OperatorId(
    Guid Value
);