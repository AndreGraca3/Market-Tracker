namespace market_tracker_webapi.Application.Repository.Dto.Operator;

public record OperatorInfo(
    Guid Id,
    string Name,
    string Email,
    int PhoneNumber,
    DateTime CreatedAt
);