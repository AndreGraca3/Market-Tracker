﻿namespace market_tracker_webapi.Application.Domain.Schemas.Account.Users;

public record Operator(
    UserId Id,
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
        new UserId(Id),
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
}

public record OperatorItem(UserId Id, string Name, int PhoneNumber, string StoreName)
{
    public OperatorItem(Guid id, string name, int phoneNumber, string storeName) : this(
        new UserId(id),
        name,
        phoneNumber,
        storeName
    )
    {
    }
}