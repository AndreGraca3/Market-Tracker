using market_tracker_webapi.Application.Domain.Schemas.Account.Users;

namespace market_tracker_webapi.Application.Domain.Schemas.Account.Auth;

public record PreRegistration(
    PreRegistrationCode Code,
    string OperatorName,
    string Email,
    int PhoneNumber,
    string StoreName,
    string CompanyName,
    string CompanyLogoUrl,
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
        string companyLogoUrl,
        string StoreAddress,
        string? CityName,
        string Document,
        DateTime CreatedAt,
        bool IsValidated
    ) : this(
        new PreRegistrationCode(Code),
        OperatorName,
        Email,
        PhoneNumber,
        StoreName,
        CompanyName,
        companyLogoUrl,
        StoreAddress,
        CityName,
        Document,
        CreatedAt,
        IsValidated
    )
    {
    }
}

public record PreRegistrationCode(
    Guid Value
);