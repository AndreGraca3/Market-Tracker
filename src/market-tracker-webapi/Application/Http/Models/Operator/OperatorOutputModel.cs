namespace market_tracker_webapi.Application.Http.Models.Operator;

public record OperatorOutputModel(
    Guid Id,
    string Username,
    string Name,
    string Email,
    DateTime CreatedAt,
    int? PhoneNumber
);