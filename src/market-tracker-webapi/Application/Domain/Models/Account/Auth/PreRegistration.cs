namespace market_tracker_webapi.Application.Domain.Models.Account.Auth;

public record PreRegistration(
    PreRegistrationId Code,
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
)
{
    public PreRegistration(
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
    ) : this(
        new PreRegistrationId(Code),
        OperatorName,
        Email,
        PhoneNumber,
        StoreName,
        CompanyName,
        StoreAddress,
        CityName,
        Document,
        CreatedAt,
        IsValidated
    )
    {
    }
}

public record PreRegistrationId(
    Guid Value
);