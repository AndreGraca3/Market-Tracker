namespace market_tracker_webapi.Application.Domain.Models.Account.Auth;

public record PreRegistration(
    Guid Code,
    string OperatorName,
    string Email,
    int PhoneNumber,
    string StoreName,
    string CompanyName,
    string StoreAddress,
    string? CityName,
    string Document,
    DateTime CreatedAt,
    bool IsValidated
);