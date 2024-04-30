namespace market_tracker_webapi.Application.Repository.Dto.Operator;

public record OperatorInfo(
    Guid Id,
    string Username,
    string Name,
    string Email,
    DateTime CreatedAt,
    int PhoneNumber
);