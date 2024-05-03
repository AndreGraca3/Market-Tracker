namespace market_tracker_webapi.Application.Domain;

public record PreRegister(
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